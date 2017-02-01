using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal enum Command
    {
        Command1,
        Command2
    }
    internal class Groups1
    {
        [ActionArgument]
        public Command Command { get; set; }

        [ArgumentGroup(nameof(Command.Command1))]
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public string p1 { get; set; }

        [ArgumentGroup(nameof(Command.Command1))]
        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public int opt1 { get; set; }

        [ArgumentGroup(nameof(Command.Command2))]
        [RequiredArgument(0, "opt1", "Optional parameter 1")]
        public Enum1 opt11 { get; set; }

        [ArgumentGroup(nameof(Command.Command2))]
        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public Enum1 opt2 { get; set; }
    }
}
