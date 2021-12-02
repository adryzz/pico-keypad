namespace KeypadConfigurator.Data
{
    public static class KeypadConfigurationValidator
    {
        private const int PicoMaxPin = 40;

        private static readonly int[] ReservedPins = {
            3, 8, 13, 18, 23, 28, 30, 33, 35, 36, 37, 38, 39, 40
        };
            
        public static bool Validate(KeypadConfiguration config)
        {
            var fails = config.FriendlyName.Length > ushort.MaxValue;
            
            fails = fails || checkPin(config.LeftKey.Pin);
            fails = fails || checkPin(config.RightKey.Pin);

            fails = fails || checkKey(config.LeftKey.KeyChar);
            fails = fails || checkKey(config.RightKey.KeyChar);

            fails = fails || config.LeftKey.DebounceTime < 0;
            fails = fails || config.RightKey.DebounceTime < 0;
            
            return !fails;
        }

        private static bool checkPin(byte pin)
        {
            if (pin > PicoMaxPin)
                return false;
            
            for (int i = 0; i < ReservedPins.Length; i++)
            {
                if (pin == ReservedPins[i])
                    return false;
            }

            return true;
        }

        private static bool checkKey(char key)
        {
            return char.IsLetterOrDigit(key);
        }
    }
}