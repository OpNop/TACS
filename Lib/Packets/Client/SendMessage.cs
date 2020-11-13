using TinySocket;

namespace TACSLib.Packets
{
    public class SendMessage
    {
        public string Message;

        private const PacketType ID = PacketType.C_SEND_MESSAGE;

        public SendMessage() { }

        public SendMessage(Unpacker p)
        {
            Message = p.GetString();
        }

        public byte[] Write()
        {
            var p = new Packer((byte)ID);
            p.AddString(Message);
            return p.ToArray();
        }
    }
}
