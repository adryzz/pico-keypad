namespace KeypadConfigurator.Data
{
    public struct KeypadConfiguration
    {
        /// <summary>
        /// The Vendor ID of the keypad.
        /// </summary>
        public ushort Vid { get; set; }
        
        /// <summary>
        /// The Product ID of the keypad.
        /// </summary>
        public ushort Pid { get; set; }

        /// <summary>
        /// The friendly name of the keypad.
        /// </summary>
        /// <remarks>
        /// Max length is <see cref="ushort"/>.<see cref="ushort.MaxValue"/>, but only a max size of 255 is officially supported.
        /// </remarks>
        public string FriendlyName { get; set; }
        
        /// <summary>
        /// The left key of the keypad.
        /// </summary>
        public KeyConfiguration LeftKey { get; set; }
        
        /// <summary>
        /// The right key of the keypad.
        /// </summary>
        public KeyConfiguration RightKey { get; set; }
    }
}