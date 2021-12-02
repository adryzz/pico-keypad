namespace KeypadConfigurator.Data
{
    public struct KeypadConfiguration
    {
        public ushort Vid { get; set; }
        public ushort Pid { get; set; }

        public string FriendlyName { get; set; }

        public KeyConfiguration LeftKey { get; set; }
        public KeyConfiguration RightKey { get; set; }
    }
}