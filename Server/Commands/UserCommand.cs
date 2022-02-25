using System.Linq;
using System.Threading.Tasks;
using TACS_Server.User;
using TACSLib.Packets.Server;

namespace TACS_Server.Commands
{
    public static class UserCommand
    {
        public static async Task Afk(UserSession user, UserSessionList userList, string args)
        {

        }

        public static async Task Admin(UserSession user, UserSessionList userList, string args)
        {
            user.IsOfficer = true;
        }

        public static async Task List(UserSession user, UserSessionList userList, string args)
        {
            await Task.Run(() =>
            {
                var list = userList.sessionList.Values.OfType<UserSession>().Select(u => u.CharacterName);
                user.Send(new ServerSendMessage($"Online users: {string.Join(", ", list)}"));
            });
        }
    }
}
