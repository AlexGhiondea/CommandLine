using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class Groups_NoCommand
    {
        public Command Command { get; set; }

        [ArgumentGroup(nameof(Command.Command1))]
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public string p1 { get; set; }

        [ArgumentGroup(nameof(Command.Command1))]
        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public int opt1 { get; set; }
    }
}
