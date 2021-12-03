namespace KeypadConfigurator.Data
{
    public class ErrorPacket : IPacket
    {
        public PacketType Type => PacketType.Error;
    }
}