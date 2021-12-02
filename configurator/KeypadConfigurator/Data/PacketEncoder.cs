using System;
using System.Text;

namespace KeypadConfigurator.Data
{
    public static class PacketEncoder
    {
        public static byte[] Encode(IPacket packet)
        {
            if (packet.Type == PacketType.HostHandshake)
            {
                return new[] { (byte)packet.Type };
            }

            if (packet.Type == PacketType.Configuration)
            {
                ConfiguratonPacket toEncode = (ConfiguratonPacket) packet;
                int size = sizeof(PacketType) //packet type
                           + sizeof(ushort) //VID
                           + sizeof(ushort) //PID
                           + sizeof(ushort) //friendly name length
                           + toEncode.Configuration.FriendlyName.Length //friendly name
                           + sizeof(byte) //pin
                           + sizeof(char) //key
                           + sizeof(double) //debounce time
                           + sizeof(byte) //pin
                           + sizeof(char) //key
                           + sizeof(double); //debounce time

                byte[] data = new byte[size];

                int index = 0;
                
                data[0] = (byte)toEncode.Type;
                index+= sizeof(byte);
                
                BitConverter.GetBytes(toEncode.Configuration.Vid).CopyTo(data, index);
                index += sizeof(ushort);
                
                BitConverter.GetBytes(toEncode.Configuration.Vid).CopyTo(data, index);
                index += sizeof(ushort);
                
                BitConverter.GetBytes((ushort)toEncode.Configuration.FriendlyName.Length).CopyTo(data, index);
                index += sizeof(ushort);
                
                Encoding.ASCII.GetBytes(toEncode.Configuration.FriendlyName).CopyTo(data, index);
                index += toEncode.Configuration.FriendlyName.Length;

                data[index] = toEncode.Configuration.LeftKey.Pin;
                index += sizeof(byte);

                data[index] = (byte)toEncode.Configuration.LeftKey.KeyChar;
                index += sizeof(char);
                
                BitConverter.GetBytes(toEncode.Configuration.LeftKey.DebounceTime).CopyTo(data, index);
                index += sizeof(double);
                
                data[index] = toEncode.Configuration.RightKey.Pin;
                index += sizeof(byte);

                data[index] = (byte)toEncode.Configuration.RightKey.KeyChar;
                index += sizeof(char);
                
                BitConverter.GetBytes(toEncode.Configuration.RightKey.DebounceTime).CopyTo(data, index);
                index += sizeof(double);

                return data;

            }
            return Array.Empty<byte>();
        }

        public static IPacket Decode(byte[] packet)
        {
            if (packet.Length == 0)
                throw new ArgumentException();
            
            PacketType type = (PacketType)packet[0];

            if (type == PacketType.ClientHandshake)
                return new ClientHandshakePacket();

            return null;
        }
    }
}