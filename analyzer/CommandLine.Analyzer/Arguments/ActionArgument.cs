using System.Collections.Generic;

namespace CommandLine.Analyzer
{
    internal class ActionArgument : Argument
    {
        public List<string> Values { get; set; } = new List<string>();
    }
}
