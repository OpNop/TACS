using TinyTools.TinySocket;

namespace TACSLib.Packets.Server
{
    public class ServerVersion : IPacket
    {
        private readonly PacketType _packetType = PacketType.S_SERVER_VER;

        public string ServerRSA { get; private set; }

        public ServerVersion(string serverRSA)
        {
            ServerRSA = serverRSA;
        }

        public ServerVersion(Unpacker p)
        {
            ServerRSA = p.GetASCIIString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)_packetType);
            p.AddString(ServerRSA);
            return p.ToArray();
        }

        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
