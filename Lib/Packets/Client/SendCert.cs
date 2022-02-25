using TinySocket;

namespace TACSLib.Packets.Client
{
    public class SendCert
    {
        private readonly PacketType _packetType = PacketType.C_SEND_CERT;

        public string RSAKey { get; private set; }

        public SendCert(string rsaKey)
        {
            RSAKey = rsaKey;
        }

        public SendCert(Unpacker p)
        {
            RSAKey = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)_packetType);
            p.AddString(RSAKey);
            return p.ToArray();
        }
        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
