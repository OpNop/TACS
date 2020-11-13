using TinySocket;

namespace TACSLib.Packets
{
    public class Login
    {
        public int ClientVersion;
        public string APIKey;

        private const PacketType ID = PacketType.C_LOGIN;

        public Login() { }

        public Login(Unpacker p)
        {
            ClientVersion = p.GetInt32();
            APIKey = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)ID);
            p.Add(ClientVersion);
            p.AddString(APIKey);
            return p.ToArray();
        }
    }
}
