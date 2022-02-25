namespace TACSLib.Packets
{
    public interface IPacket
    {
        PacketType ID { get; }
        public byte[] Pack();
    }
}
