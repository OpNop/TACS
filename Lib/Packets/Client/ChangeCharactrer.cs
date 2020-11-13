using TinySocket;

namespace TACSLib.Packets
{
    public class ChangeCharacter
    {
        public string CharacterName;

        private const PacketType ID = PacketType.C_CHANGE_CHARACTER;

        public ChangeCharacter() { }

        public ChangeCharacter(Unpacker p)
        {
            CharacterName = p.GetString();
        }

        public byte[] Write()
        {
            var p = new Packer((byte)ID);
            p.AddString(CharacterName);
            return p.ToArray();
        }
    }
}
