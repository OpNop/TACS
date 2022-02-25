using TinyTools.TinySocket;

namespace TACSLib.Packets.Server
{
    public class ServerSendMessage : IPacket
    {
        private readonly PacketType _packetType = PacketType.S_SEND_MESSAGE;

        public MessageType Type { get; private set; }
        public string CharacterName { get; private set; }
        public string Message { get; private set; }


        public ServerSendMessage(string message)
        {
            Type = MessageType.SYSTEM;
            CharacterName = "";
            Message = message;
        }

        public ServerSendMessage(string message, string characterName, MessageType type)
        {
            Type = type;
            CharacterName = characterName;
            Message = message;
        }

        public ServerSendMessage(Unpacker p)
        {
            Type = (MessageType)p.GetInt8();
            CharacterName = p.GetString();
            Message = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)_packetType);
            p.Add((byte)Type);
            p.AddString(CharacterName);
            p.AddString(Message);
            return p.ToArray();
        }

        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
