using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TACS_Server.User;
using TACSLib.Packets.Server;

namespace TACS_Server.Commands
{
   internal static class UserCommandsBuilder
   {
      internal static ChatCommandHandler BuildUserCommands(this ChatCommandHandler handler)
      {
         handler.RegisterCommand(new UserChatCommand("afk", (UserSession user, UserSessionList userList, string args) => throw new NotImplementedException()));
         handler.RegisterCommand(new UserChatCommand("admin", async (UserSession user, UserSessionList userList, string args) =>
            {
               user.IsOfficer = true;
            }));
         handler.RegisterCommand(new UserChatCommand("list", async (UserSession user, UserSessionList userList, string args) =>
            {
               await Task.Run(() =>
               {
                   IEnumerable<string> list;

                   if (user.IsOfficer)
                   {
                       list = userList.sessionList.Values.OfType<UserSession>().Select(u => u.CharacterName);
                   } else
                   {
                       list = userList.sessionList.Values.OfType<UserSession>().Where(u => !u.IsHidden).Select(u => u.CharacterName);
                   }
                  
                  user.Send(new ServerSendMessage($"{list.Count()} Online users: {string.Join(", ", list)}"));
               });
            }));

         return handler;
      }
   }
}

