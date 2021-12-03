namespace KeypadConfigurator.Data
{
    public class OkPacket : IPacket
    {
        public PacketType Type => PacketType.Ok;
    }
}