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

      protected ChatCommand(string commandText, CommandType commandType, Func<UserSession, UserSessionList, string, Task> commandBody)
      {
         CommandText = commandText;
         CommandType = commandType;
         _commandBody = commandBody;
      }

      internal string CommandText { get; private set; }
      internal CommandType CommandType { get; private set; }

      internal async Task Execute(UserSession user, UserSessionList userList, string args)
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
   }

   internal class UserChatCommand : ChatCommand
   {
      internal UserChatCommand(string commandText, Func<UserSession, UserSessionList, string, Task> commandBody)
           : base(commandText, CommandType.User, commandBody)
      {
      }
   }

   internal class EmoteCommand : UserChatCommand
   {
      internal EmoteCommand(string commandText, Func<UserSession, string> emoteTextFormatter) : base(commandText, async (UserSession user, UserSessionList userList, string args) =>
         {
            await userList.Broadcast(new ServerSendMessage(emoteTextFormatter(user)));
         })
      {
      }
   }

   internal class TargetableEmoteCommand : UserChatCommand
   {
      internal TargetableEmoteCommand(string commandText, Func<UserSession, string> emoteTextFormatter, Func<UserSession, string, string> targetedEmoteTextFormatter) : base(commandText, async (UserSession user, UserSessionList userList, string args) =>
         {
            if (!string.IsNullOrEmpty(args))
               await userList.Broadcast(new ServerSendMessage(emoteTextFormatter(user)));
            else
               await userList.Broadcast(new ServerSendMessage(targetedEmoteTextFormatter(user, args)));
         })
      {
      }
   }


}
