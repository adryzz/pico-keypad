namespace KeypadConfigurator.Data
{
    public class ConfigurationRequestPacket : IPacket
    {
        public PacketType Type => PacketType.ConfigurationRequest;
    }
}