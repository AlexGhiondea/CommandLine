namespace CommandLine.Analyzer
{
    internal class NamedArgument : Argument
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCollection { get; set; }
    }
}
