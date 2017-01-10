using CommandLine.Attributes;
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
        [CommandArgument("command", "The command to perform")]
        public Command Command { get; set; }

        [CommandGroupArgument(nameof(Command.Command1))]
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public string p1 { get; set; }

        [CommandGroupArgument(nameof(Command.Command1))]
        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public int opt1 { get; set; }
    }
}
