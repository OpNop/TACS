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

        public void Add(UserSession s)
        {
            lock (this)
            {
                sessionList.Add(s.AccountName, s);
            }
        }

        public UserSession GetUserByCharacter(string characterName)
        {
            lock (this)
            {
                return sessionList.Values.OfType<UserSession>().Where(u => u.CharacterName.ToLower() == characterName.ToLower()).FirstOrDefault();
            }
        }

        public bool Exist(string accountName)
        {
            lock (this)
            {
                return sessionList.ContainsKey(accountName);
            }
        }

        public UserSession GetUserSession(string accountName)
        {
            return (UserSession)sessionList[accountName];
        }

        public async Task Remove(UserSession us)
        {
            if (us.IsAuthenticated)
            {
                lock (this)
                {
                    sessionList.Remove(us.AccountName);
                }

                //Broadcast that the user has logged off
                await Broadcast(new StatusChange(us.AccountName, StatusType.Offline));
            }
        }

        public async Task Broadcast(IServerPacket Packet)
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
