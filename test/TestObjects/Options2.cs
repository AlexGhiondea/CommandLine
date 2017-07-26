using CommandLine.Attributes;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class Options2
    {
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public string p1 { get; set; }

        [RequiredArgument(1, "p2", "Required parameter 2", true)]
        public List<string> p2 { get; set; }

        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public int opt1 { get; set; }

        [OptionalArgument(null, "opt2", "Optional parameter 2", true)]
        public List<string> opt2 { get; set; }

        [OptionalArgument('a', "opt3", "Optional parameter 3")]
        public char Character { get; set; }
    }
}
