using System.Threading.Tasks;
using TACS_Server.User;
using TACSLib.Packets.Server;

namespace TACS_Server.Commands
{
   internal static class AdminCommandsBuilder
   {
      internal static ChatCommandHandler BuildAdminCommands(this ChatCommandHandler handler)
      {
         handler.RegisterCommand(new AdminChatCommand("mute", async (UserSession user, UserSessionList userList, string args) =>
            {
               if (string.IsNullOrEmpty(args))
               {
                  user.Send(new ServerSendMessage("Usage: /.mute <Charater Name>"));
                  return;
               }

               var target = await userList.GetUserByCharacter(string.Join(" ", args));

               if (target != null)
               {
                  target.IsMuted = true;
                  user.Send(new ServerSendMessage($"User {target.AccountName} has been muted"));
               }
               else
               {
                  user.Send(new ServerSendMessage("User not found"));
               }
            }));

         handler.RegisterCommand(new AdminChatCommand("unmute", async (UserSession user, UserSessionList userList, string args) =>
            {
               if (string.IsNullOrEmpty(args))
               {
                  user.Send(new ServerSendMessage("Usage: /.unmute <Charater Name>"));
                  return;
               }

               var target = await userList.GetUserByCharacter(args);

               if (target != null)
               {
                  target.IsMuted = false;
                  user.Send(new ServerSendMessage($"User {target.AccountName} has been unmuted"));
               }
               else
               {
                  user.Send(new ServerSendMessage("User not found"));
               }
            }));

         handler.RegisterCommand(new AdminChatCommand("kick", async(UserSession user, UserSessionList userList, string args) =>
            {
                  if (string.IsNullOrEmpty(args))
                  {
                     user.Send(new ServerSendMessage("Usage: /.kick <Charater Name>"));
                     return;
                  }

                  var target = await userList.GetUserByCharacter(args);

                  if (target != null)
                  {
                     target.Stop();
                  }
                  else
                  {
                     user.Send(new ServerSendMessage("User not found"));
                  }
               }));

         handler.RegisterCommand(new AdminChatCommand("announce", async(UserSession user, UserSessionList userList, string args) =>
         {
            await userList.Broadcast(new ServerSendMessage(args.ToString(), user.CharacterName, TACSLib.MessageType.ANNOUNCE));
         }));

         //just returning this for those cool fluent builder points
         return handler;
      }
   }
}
