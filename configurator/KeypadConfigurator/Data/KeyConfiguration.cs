namespace KeypadConfigurator.Data
{
    public struct KeyConfiguration
    {
        public byte Pin { get; set; }
        public double DebounceTime { get; set; }
        public char KeyChar { get; set; }
    }
}