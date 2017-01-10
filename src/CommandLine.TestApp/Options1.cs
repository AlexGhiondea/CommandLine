using CommandLine.Attributes;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class Options1
    {
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public string p1 { get; set; }

        [RequiredArgument(1, "p2", "Required parameter 2")]
        public int p2 { get; set; }

        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public int opt1 { get; set; }

        [OptionalArgument("all", "opt2", "Optional parameter 2")]
        public string opt2 { get; set; }

        [OptionalArgument(null, "opt3", "Optional parameter 2", true)]
        public List<string> opt3 { get; set; }

    }

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
