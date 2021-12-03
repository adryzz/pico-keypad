namespace KeypadConfigurator.Data
{
    public struct KeyConfiguration
    {
        /// <summary>
        /// The pin used to connect the button.
        /// </summary>
        public byte Pin { get; set; }
        
        /// <summary>
        /// The debounce time in microseconds.
        /// </summary>
        public int DebounceTime { get; set; }
        
        /// <summary>
        /// The character the key will press.
        /// </summary>
        public char KeyChar { get; set; }
    }
}