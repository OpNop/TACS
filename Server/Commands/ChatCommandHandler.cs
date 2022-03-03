using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TACS_Server.Commands;
using TACS_Server.User;
using TACSLib.Packets.Server;

namespace TACS_Server
{
   public class ChatCommandHandler
   {
      public int UserCommandCount() => _commands.Values.Where(c => c.CommandType == CommandType.User).Count();
      public int AdminCommandCount() => _commands.Values.Where(c => c.CommandType == CommandType.Admin).Count();

      private readonly UserSessionList UserList;
      private Dictionary<string, ChatCommand> _commands = new Dictionary<string, ChatCommand>();

      public ChatCommandHandler(UserSessionList userList)
      {
         UserList = userList;
      }

      public async Task ExecuteCommand(UserSession user, string commandText, string args)
      {

         if (!_commands.ContainsKey(commandText))
         {
            user.Send(new ServerSendMessage("Command not found"));
            return;
         }

         await _commands[commandText].Execute(user, UserList, args);
      }

      internal void RegisterCommand(ChatCommand command)
      {
         foreach (var synonym in command.CommandTexts)
         {
            _commands[synonym] = command;
         }
      }
    }
}
