using TinySocket;

namespace TACSLib.Packets
{
    public class SendCert
    {
        public readonly string RSAKey;

        private const PacketType ID = PacketType.C_SEND_CERT;

        public SendCert(string RSAKey)
        {
            this.RSAKey = RSAKey;
        }

        public SendCert(Unpacker p)
        {
            RSAKey = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)ID);
            p.AddString(RSAKey);
            return p.ToArray();
        }
    }
}
