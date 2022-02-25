using TinyTools.TinySocket;

namespace TACSLib.Packets.Client
{
    public class ChangeCharacter : IPacket
    {
        private readonly PacketType _packetType = PacketType.C_CHANGE_CHARACTER;
        public string CharacterName { get; private set; }

        public ChangeCharacter(string characterName)
        {
            CharacterName = characterName;
        }

        public ChangeCharacter(Unpacker p)
        {
            CharacterName = p.GetString();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)_packetType);
            p.AddString(CharacterName);
            return p.ToArray();
        }

        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
