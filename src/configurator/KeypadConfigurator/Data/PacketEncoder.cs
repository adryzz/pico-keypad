using System;
using System.IO;
using System.Text;

namespace KeypadConfigurator.Data
{
    public static class PacketEncoder
    {
        public static byte[] Encode(IPacket packet)
        {
            if (packet.Type is PacketType.HostHandshake
                or PacketType.ClientHandshake
                or PacketType.Ok
                or PacketType.Error
                or PacketType.ConfigurationRequest)
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
                           + sizeof(int) //debounce time
                           + sizeof(byte) //pin
                           + sizeof(char) //key
                           + sizeof(int); //debounce time

                byte[] data = new byte[size];

                int index = 0;
                
                data[0] = (byte)toEncode.Type;
                index+= sizeof(byte);
                
                BitConverter.GetBytes(toEncode.Configuration.Vid).CopyTo(data, index);
                index += sizeof(ushort);
                
                BitConverter.GetBytes(toEncode.Configuration.Pid).CopyTo(data, index);
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
                index += sizeof(int);
                
                data[index] = toEncode.Configuration.RightKey.Pin;
                index += sizeof(byte);

                data[index] = (byte)toEncode.Configuration.RightKey.KeyChar;
                index += sizeof(char);
                
                BitConverter.GetBytes(toEncode.Configuration.RightKey.DebounceTime).CopyTo(data, index);
                index += sizeof(int);

                return data;

            }
            return Array.Empty<byte>();
        }

        public static IPacket Decode(byte[] packet)
        {
            if (packet.Length == 0)
                throw new ArgumentException();
            
            PacketType type = (PacketType)packet[0];

            if (type == PacketType.HostHandshake)
                return new HostHandshakePacket();
            
            if (type == PacketType.ClientHandshake)
                return new ClientHandshakePacket();

            if (type == PacketType.Ok)
                return new OkPacket();

            if (type == PacketType.Error)
                return new ErrorPacket();

            if (type == PacketType.ConfigurationRequest)
                return new ConfigurationRequestPacket();

            if (type == PacketType.Configuration)
            {
                int index = 1;
                KeypadConfiguration config = new KeypadConfiguration();

                config.Vid = BitConverter.ToUInt16(packet, index);
                index += sizeof(ushort);
                
                config.Pid = BitConverter.ToUInt16(packet, index);
                index += sizeof(ushort);

                ushort friendlyNameLength = BitConverter.ToUInt16(packet, index);
                index += sizeof(ushort);

                byte[] name = new byte[friendlyNameLength];
                Array.Copy(packet, index, name, 0, friendlyNameLength);
                
                config.FriendlyName = Encoding.ASCII.GetString(name);
                index += friendlyNameLength;

                KeyConfiguration c0 = new KeyConfiguration();
                
                c0.Pin = packet[index];
                index += sizeof(byte);

                c0.KeyChar = (char)packet[index];
                index += sizeof(char);
                
                c0.DebounceTime = BitConverter.ToInt32(packet, index);
                index += sizeof(int);
                
                KeyConfiguration c1 = new KeyConfiguration();
                
                c1.Pin = packet[index];
                index += sizeof(byte);

                c1.KeyChar = (char)packet[index];
                index += sizeof(char);
                
                c1.DebounceTime = BitConverter.ToInt32(packet, index);
                index += sizeof(int);

                config.LeftKey = c0;
                config.RightKey = c1;

                return new ConfiguratonPacket
                {
                    Configuration = config
                };
            }

            return null;
        }
        
        public static IPacket Decode(Stream packetStream)
        {
            if (!packetStream.CanRead)
                throw new ArgumentException();
            
            PacketType type = (PacketType)packetStream.ReadByte();

            if (type == PacketType.HostHandshake)
                return new HostHandshakePacket();
            
            if (type == PacketType.ClientHandshake)
                return new ClientHandshakePacket();

            if (type == PacketType.Ok)
                return new OkPacket();

            if (type == PacketType.Error)
                return new ErrorPacket();

            if (type == PacketType.ConfigurationRequest)
                return new ConfigurationRequestPacket();

            if (type == PacketType.Configuration)
            {
                int index = 1;
                KeypadConfiguration config = new KeypadConfiguration();

                byte[] vid = new byte[sizeof(ushort)];
                packetStream.Read(vid, 0, vid.Length);
                config.Vid = BitConverter.ToUInt16(vid, 0);

                byte[] pid = new byte[sizeof(ushort)];
                packetStream.Read(pid, 0, pid.Length);
                config.Pid = BitConverter.ToUInt16(pid, 0);

                byte[] length = new byte[sizeof(ushort)];
                packetStream.Read(length, 0, length.Length);
                ushort friendlyNameLength = BitConverter.ToUInt16(length, 0);

                byte[] name = new byte[friendlyNameLength];
                packetStream.Read(name, 0, friendlyNameLength);
                config.FriendlyName = Encoding.ASCII.GetString(name);

                KeyConfiguration c0 = new KeyConfiguration();

                c0.Pin = (byte)packetStream.ReadByte();
                c0.KeyChar = (char)packetStream.ReadByte();

                byte[] d0 = new byte[sizeof(int)];
                packetStream.Read(d0, 0, d0.Length);
                c0.DebounceTime = BitConverter.ToInt32(d0, index);

                KeyConfiguration c1 = new KeyConfiguration();
                
                c1.Pin = (byte)packetStream.ReadByte();
                c1.KeyChar = (char)packetStream.ReadByte();
                
                byte[] d1 = new byte[sizeof(int)];
                packetStream.Read(d1, 0, d1.Length);
                c0.DebounceTime = BitConverter.ToInt32(d1, index);

                config.LeftKey = c0;
                config.RightKey = c1;

                return new ConfiguratonPacket
                {
                    Configuration = config
                };
            }

            return null;
        }
    }
}