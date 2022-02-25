using TinyTools.TinySocket;

namespace TACSLib.Packets.Client
{
    public class Login : IPacket
    {
        private readonly PacketType _packetType = PacketType.C_LOGIN;

        public int ClientVersion { get; private set; }
        public string APIKey { get; private set; }

        public Login(int clientVersion, string apiKey)
        {
            ClientVersion = clientVersion;
            APIKey = apiKey;
        }

        public Login(Unpacker p)
        {
            ClientVersion = p.GetInt32();
            APIKey = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)_packetType);
            p.Add(ClientVersion);
            p.AddString(APIKey);
            return p.ToArray();
        }

        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
