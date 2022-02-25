using System.Drawing;
using TinyTools.TinySocket;

namespace TACSLib
{
    public class MumbleData
    {
        public string CharacterName;
        public string Race;
        public string Profession;
        public string EliteSpec;
        public int Map;
        public Point Position;
        public string ServerAddress;

        public void Update(Unpacker data)
        {
            CharacterName = data.GetString(); ;
            Race = data.GetString();
            EliteSpec = data.GetString();
            Map = data.GetInt32();
            Position = new Point(data.GetInt32(), data.GetInt32());
            ServerAddress = data.GetString();
        }
    }
}
