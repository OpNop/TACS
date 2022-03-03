using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TACS_Server.User;
using TACSLib.Packets.Server;

namespace TACS_Server.Commands
{
   internal class ChatCommand
   {
      private Func<UserSession, UserSessionList, string, Task> _commandBody;

      protected ChatCommand(ICollection<string> commandTexts, CommandType commandType, Func<UserSession, UserSessionList, string, Task> commandBody)
      {
         CommandTexts = commandTexts;
         CommandType = commandType;
         _commandBody = commandBody;
      }

      protected ChatCommand(string commandText, CommandType commandType, Func<UserSession, UserSessionList, string, Task> commandBody) : this(new List<string> {  commandText }, commandType, commandBody)
      {
      }

      internal ICollection<string> CommandTexts { get; private set; }
      internal CommandType CommandType { get; private set; }

      internal virtual async Task Execute(UserSession user, UserSessionList userList, string args)
      {
         await _commandBody(user, userList, args);
      }
   }

   internal class AdminChatCommand : ChatCommand
   {
      internal AdminChatCommand(string commandText, Func<UserSession, UserSessionList, string, Task> commandBody) 
           : base($".{commandText}",CommandType.Admin, commandBody)
      {
      }

      internal override async Task Execute(UserSession user, UserSessionList userList, string args)
      {
         if (user.IsOfficer)
         {
            await base.Execute(user, userList, args);
         }
         else
         {
            user.Send(new ServerSendMessage("Command not found"));
         }   
      }
   }

   internal class UserChatCommand : ChatCommand
   {
      internal UserChatCommand(ICollection<string> commandTexts, Func<UserSession, UserSessionList, string, Task> commandBody)
     : base(commandTexts, CommandType.User, commandBody)
      {
      }

      internal UserChatCommand(string commandText, Func<UserSession, UserSessionList, string, Task> commandBody)
           : base(commandText, CommandType.User, commandBody)
      {
      }
   }

   internal class EmoteCommand : UserChatCommand
   {
      internal EmoteCommand(ICollection<string> commandTexts, Func<UserSession, string> emoteTextFormatter) : base(commandTexts, async (UserSession user, UserSessionList userList, string args) =>
      {
         await userList.Broadcast(new ServerSendMessage(emoteTextFormatter(user)));
      })
      {
      }

      internal EmoteCommand(string commandText, Func<UserSession, string> emoteTextFormatter) : base(commandText, async (UserSession user, UserSessionList userList, string args) =>
         {
            await userList.Broadcast(new ServerSendMessage(emoteTextFormatter(user)));
         })
      {
      }
   }

   internal class TargetableEmoteCommand : UserChatCommand
   {
      internal TargetableEmoteCommand(ICollection<string> commandTexts, Func<UserSession, string> emoteTextFormatter, Func<UserSession, string, string> targetedEmoteTextFormatter) : base(commandTexts, async (UserSession user, UserSessionList userList, string args) =>
      {
         if (string.IsNullOrEmpty(args))
            await userList.Broadcast(new ServerSendMessage(emoteTextFormatter(user)));
         else
            await userList.Broadcast(new ServerSendMessage(targetedEmoteTextFormatter(user, args)));
      })
      {
      }

      internal TargetableEmoteCommand(string commandText, Func<UserSession, string> emoteTextFormatter, Func<UserSession, string, string> targetedEmoteTextFormatter) : base(commandText, async (UserSession user, UserSessionList userList, string args) =>
         {
            if (string.IsNullOrEmpty(args))
               await userList.Broadcast(new ServerSendMessage(emoteTextFormatter(user)));
            else
               await userList.Broadcast(new ServerSendMessage(targetedEmoteTextFormatter(user, args)));
         })
      {
      }
   }


}
