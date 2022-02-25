using TinyTools.TinySocket;

namespace TACSLib.Packets.Server
{
    public class LoginResult : IPacket
    {
        private readonly PacketType _packetType = PacketType.S_LOGIN_RESULT;
        public byte AuthCode { get; private set; }
        public string AuthMessage { get; private set; }
        
        public LoginResult(byte authCode)
        {
            AuthCode = authCode;
            AuthMessage = "";
        }

        public LoginResult(byte authCode, string authMessage)
        {
            AuthCode = authCode;
            AuthMessage = authMessage;
        }

        public LoginResult(Unpacker p)
        {
            AuthCode = p.GetUInt8();
            AuthMessage = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)_packetType);
            p.Add(AuthCode);
            p.AddString(AuthMessage);
            return p.ToArray();
        }

        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
