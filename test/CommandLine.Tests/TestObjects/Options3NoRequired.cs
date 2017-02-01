using CommandLine.Attributes;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class Options3NoRequired
    {
        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public int opt1 { get; set; }

        [OptionalArgument(null, "opt2", "Optional parameter 2", true)]
        public List<string> opt2 { get; set; }

        [OptionalArgument(Enum1.B, "opt3", "Optional parameter 3")]
        public Enum1 opt3 { get; set; }
    }

    internal enum Enum1
    {
        A,
        B
    }
}
