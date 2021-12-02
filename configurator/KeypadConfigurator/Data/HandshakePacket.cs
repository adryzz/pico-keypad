namespace KeypadConfigurator.Data
{
    public class HandshakePacket : IPacket
    {
        public PacketType Type { get; }
    }

    public class HostHandshakePacket : HandshakePacket
    {
        public PacketType Type => PacketType.HostHandshake;
    }

    public class ClientHandshakePacket : HandshakePacket
    {
        public PacketType Type => PacketType.ClientHandshake;
    }
}