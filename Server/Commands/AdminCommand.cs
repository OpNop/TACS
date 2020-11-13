using System.Threading.Tasks;
using TACSLib.Packets;
using TACSLib.Packets.Server;

namespace TACS_Server.Commands
{
    public static class AdminCommand
    {
        [Command("mute")]
        [IsAdmin]
        public static async Task Mute(UserSession user, UserSessionList userList, string args)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(args))
                {
                    user.Send(new Message("Usage: /.mute <Charater Name>"));
                    return;
                }

                var target = userList.GetUserByCharacter(string.Join(" ", args));

                if (target != null)
                {
                    target.IsMuted = true;
                    user.Send(new Message($"User {target.AccountName} has been muted"));
                }
                else
                {
                    user.Send(new Message("User not found"));
                }
            });
        }

        public static async Task Unmute(UserSession user, UserSessionList userList, string args)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(args))
                {
                    user.Send(new Message("Usage: /.unmute <Charater Name>"));
                    return;
                }

                var target = userList.GetUserByCharacter(args);

                if (target != null)
                {
                    target.IsMuted = false;
                    user.Send(new Message($"User {target.AccountName} has been unmuted"));
                }
                else
                {
                    user.Send(new Message("User not found"));
                }
            });
        }

        [Command("kick")]
        [IsAdmin]
        public static async Task Kick(UserSession user, UserSessionList userList, string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                user.Send(new Message("Usage: /.kick <Charater Name>"));
                return;
            }

            var target = userList.GetUserByCharacter(args);

            if (target != null)
            {
                target.Stop();
            }
            else
            {
                user.Send(new Message("User not found"));
            }
        }

        [Command("announce")]
        [IsAdmin]
        public static async Task Announce(UserSession user, UserSessionList userList, string args)
        {
            await userList.Broadcast(new Message(args.ToString(), user.CharacterName, TACSLib.MessageType.ANNOUNCE));
        }
    }
}
