using CommandLine.Attributes;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class OptionsNegative1
    {
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public Enum1 p1 { get; set; }

        [RequiredArgument(1, "p2", "Required parameter 2", true)]
        public List<string> p2 { get; set; }
    }
}
