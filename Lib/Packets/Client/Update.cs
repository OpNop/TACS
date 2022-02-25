using System.Drawing;
using TinySocket;

namespace TACSLib.Packets.Client
{
    public class Update : IPacket
    {
        private readonly PacketType _packetType = PacketType.C_UPDATE;
        public MumbleData MumbleData;

        public Update(MumbleData mumbleData)
        {
            MumbleData = mumbleData;
        }

        public Update(Unpacker p)
        {
            MumbleData.CharacterName = p.GetString();
            MumbleData.Race = p.GetString();
            MumbleData.EliteSpec = p.GetString();
            MumbleData.Map = p.GetInt32();
            MumbleData.Position = new Point(p.GetInt32(), p.GetInt32());
            MumbleData.ServerAddress = p.GetString();
        }

        public byte[] Pack()
        {
            return default;
            //var p = new Packer((byte)ID);
            //p.AddString(CharacterName);
            //return p.ToArray();
        }
        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
