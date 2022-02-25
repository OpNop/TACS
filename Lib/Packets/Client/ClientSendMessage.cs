using TinyTools.TinySocket;

namespace TACSLib.Packets.Client
{
    public class ClientSendMessage : IPacket
    {
        private readonly PacketType _packetType = PacketType.C_SEND_MESSAGE;
        public string Message { get; private set; }

        public ClientSendMessage(string message)
        {
            Message = message;
        }

        public ClientSendMessage(Unpacker p)
        {
            Message = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)_packetType);
            p.AddString(Message);
            return p.ToArray();
        }

        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
