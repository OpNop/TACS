using TinySocket;

namespace TACSLib.Packets.Server
{
    public class StatusChange : IServerPacket
    {
        private readonly string _accountName;
        private readonly StatusType _status;

        private const PacketType ID = PacketType.S_STATUS_CHANGE;

        public StatusChange(string accountName, StatusType status)
        {
            _accountName = accountName;
            _status = status;
        }

        public StatusChange(Unpacker p)
        {
            _accountName = p.GetString();
            _status = (StatusType)p.GetInt8();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)ID);
            p.AddString(_accountName);
            p.Add((byte)_status);
            return p.ToArray();
        }
    }
}
