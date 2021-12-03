using System;
using System.IO.Ports;

namespace KeypadConfigurator.Data
{
    public static class KeypadConnectionManager
    {
        private static SerialPort? port;

        public static bool Connected => _connected;
        private static bool _connected = false;

        public static void Initialize(string portName)
        {
            port = new SerialPort(portName, 115200);
            port.Open();
            _connected = true;
        }

        public static IPacket Write(IPacket packet)
        {
            if (port == null)
            {
                throw new InvalidOperationException();
            }
            
            if (packet is ConfiguratonPacket p)
            {
                if (!KeypadConfigurationValidator.Validate(p.Configuration))
                    throw new ArgumentException();
            }

            byte[] data = PacketEncoder.Encode(packet);
            
            port.Write(data, 0, data.Length);

            return PacketEncoder.Decode(port.BaseStream);
        }

        public static string[] GetAvailablePorts() => SerialPort.GetPortNames();

        public static void Close()
        {
            port?.Close();
            _connected = false;
        }
    }
}