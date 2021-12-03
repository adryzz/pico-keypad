namespace KeypadConfigurator.Data
{
    public enum PacketType : byte
    {
        HostHandshake,
        ClientHandshake,
        Ok,
        Error,
        ConfigurationRequest,
        Configuration
        
    }
}