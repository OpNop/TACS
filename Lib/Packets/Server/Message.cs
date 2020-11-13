using TinySocket;

namespace TACSLib.Packets.Server
{
    public class Message : IServerPacket
    {
        private readonly MessageType _type;
        private readonly string _characterName;
        private readonly string _message;

        private const PacketType ID = PacketType.S_SEND_MESSAGE;

        public Message(string message)
        {
            _type = MessageType.SYSTEM;
            _characterName = "";
            _message = message;
        }

        public Message(string message, string characterName, MessageType type)
        {
            _type = type;
            _characterName = characterName;
            _message = message;
        }

        public Message(Unpacker p)
        {
            _type = (MessageType)p.GetInt8();
            _characterName = p.GetString();
            _message = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)ID);
            p.Add((byte)_type);
            p.AddString(_characterName);
            p.AddString(_message);
            return p.ToArray();
        }
    }
}
