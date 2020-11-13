using TinySocket;

namespace TACSLib.Packets.Server
{
    public class LoginResult : IServerPacket
    {
        private readonly byte _authCode;
        private readonly string _authMessage;

        private const PacketType ID = PacketType.S_LOGIN_RESULT;

        public LoginResult(byte authCode)
        {
            _authCode = authCode;
            _authMessage = "";
        }

        public LoginResult(byte authCode, string authMessage)
        {
            _authCode = authCode;
            _authMessage = authMessage;
        }

        public LoginResult(Unpacker p)
        {
            _authCode = p.GetUInt8();
            _authMessage = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)ID);
            p.Add(_authCode);
            p.AddString(_authMessage);
            return p.ToArray();
        }
    }
}
