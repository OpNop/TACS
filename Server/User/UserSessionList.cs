using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TACSLib;
using TACSLib.Packets;
using TACSLib.Packets.Server;

namespace TACS_Server
{
    public class UserSessionList
    {
        private readonly ChatServer chatServer;
        public Dictionary<string, UserSession> sessionList = new Dictionary<string, UserSession>();

        public UserSessionList(ChatServer cs)
        {
            chatServer = cs;
        }

        public async Task<bool> Add(UserSession s)
        {
            sessionList.Add(s.AccountName, s);
            return await Task.FromResult(true);
        }

        public async Task<UserSession> GetUserByCharacter(string characterName)
        {
            return await Task.FromResult(sessionList.Values.OfType<UserSession>().Where(u => u.CharacterName.ToLower() == characterName.ToLower()).FirstOrDefault());
        }

        public async Task<bool> Exist(string accountName)
        {
            return await Task.FromResult(sessionList.ContainsKey(accountName));
        }

        public async Task<UserSession> GetUserSession(string accountName)
        {
            return await Task.FromResult(sessionList[accountName]);
        }

        public async Task<bool> Remove(UserSession us)
        {
            if (us.IsAuthenticated)
            {
                sessionList.Remove(us.AccountName);
                //Broadcast that the user has logged off
                await Broadcast(new StatusChange(us.AccountName, StatusType.Offline));
            }
            return await Task.FromResult(true);
        }

        public async Task Broadcast(IPacket Packet)
        {
            await Task.Run(() =>
            {
                foreach (UserSession user in sessionList.Values)
                {
                    user.Send(Packet);
                }
            });
        }

        public async Task BroadcastExcludeSender(IPacket Packet, UserSession Sender)
        {
            await Task.Run(() =>
            {
                foreach (UserSession user in sessionList.Values)
                {
                    if (user.ID != Sender.ID)
                    {
                        user.Send(Packet);
                    }
                }
            });
        }

        public int Count
        {
            get
            {
                return sessionList.Count;
            }
        }
    }
}
