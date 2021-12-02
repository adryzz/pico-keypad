namespace KeypadConfigurator.Data
{
    public class ConfiguratonPacket : IPacket
    {
        public PacketType Type => PacketType.Configuration;

        public KeypadConfiguration Configuration { get; set; }
    }
}