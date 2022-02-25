using TinySocket;

namespace TACSLib.Packets.Server
{
    public class StatusChange : IPacket
    {
        private readonly PacketType _packetType = PacketType.S_STATUS_CHANGE;
        public string AccountName { get; private set; }
        public StatusType Status { get; private set; }


        public StatusChange(string accountName, StatusType status)
        {
            AccountName = accountName;
            Status = status;
        }

        public StatusChange(Unpacker p)
        {
            AccountName = p.GetString();
            Status = (StatusType)p.GetInt8();
        }

        public byte[] Pack()
        {
            var p = new Packer((byte)_packetType);
            p.AddString(AccountName);
            p.Add((byte)Status);
            return p.ToArray();
        }

        public PacketType ID
        {
            get { return _packetType; }
        }
    }
}
