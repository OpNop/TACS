using Gw2Sharp;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TACSLib;
using TACSLib.Packets;
using TinyLogger;
using TinyTools.TinySocket;
using TinyTools.TinySocket.Server;

namespace TACS_Server.User
{
    public class UserSession : ASocket
    {
        public string ID;
        public string AccountName;
        public string APIKey;
        public string ClientVersion;
        public byte[] OneTimeKey;
        public bool IsEncrypted;
        public bool IsAuthenticated;
        public bool IsOfficer;
        public bool IsHidden;
        public bool IsMuted;
        public int Version;
        public MumbleData MumbleData;

        private readonly Logger log = Logger.GetInstance();
        public ChatServer chatServer;
        private Gw2Client apiClient;
        private RSACryptoServiceProvider mRSAClient;

        private Account apiAccount;

        private string _characterName;
        public string CharacterName
        {
            get
            {
                if (IsHidden)
                    return $"{_characterName} (Hidden)";
                else
                    return _characterName;
            }
            set { _characterName = value; }
        }

        public UserSession(ChatServer cs)
        {
            chatServer = cs;
            ID = Guid.NewGuid().ToString("N");
        }
        internal void InitClientKey(string RSAKey)
        {
            mRSAClient = new RSACryptoServiceProvider();
            mRSAClient.FromXmlString(RSAKey);
            IsEncrypted = true;
        }

        internal async Task<bool> ValidateKey()
        {
            var connection = new Connection(APIKey);
            apiClient = new Gw2Client(connection);
            try
            {
                apiAccount = await apiClient.WebApi.V2.Account.GetAsync();
                AccountName = apiAccount.Name;
            }
            catch (BadRequestException)
            {
                return false;
            }
            return true;
        }

        internal async Task<bool> ValidateGuild()
        {
            foreach (var guild in apiAccount.Guilds)
            {
                if (Program.Config.Guilds.Any(g => g.Guid == guild))
                    return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        #region Networking

        protected override void OnConnected()
        {
            log.AddDebug("UserSession::OnConnected Called");
        }

        protected override void OnConnectFailed(Exception e)
        {
            log.AddDebug("UserSession::OnConnectFailed Called");
        }

        protected override void OnDisconnected(Exception e)
        {
            chatServer.OnUserDisconnected(this);
        }

        protected override void OnReceived(byte[] packet)
        {
            if (IsEncrypted)
            {
                packet = chatServer.mRSASelf.Decrypt(packet, true);
            }

            Unpacker p = new Unpacker(packet);
            PacketType type = (PacketType)p.GetUInt8();
            log.AddBinary("<RESV", packet);
            chatServer.OnUserPacket(this, type, p);
        }

        protected override void OnSendFailed(Exception e)
        {
            log.AddDebug("UserSession::OnSendFailed Called");
        }

        public void Send(IPacket packet)
        {
            var data = packet.Pack();
            log.AddBinary($"SEND> {Enum.GetName(typeof(PacketType), packet.ID)}", data);

            if (IsEncrypted)
            {
                data = mRSAClient.Encrypt(data, true);
            }
            base.Send(data);
        }

        #endregion

        public override string ToString()
        {
            return string.Format($"[User: {AccountName}({CharacterName})]");
        }
    }
}
