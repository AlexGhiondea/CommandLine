using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class Groups2
    {
        [ActionArgument]
        public Command Command { get; set; }

        [CommonArgument] // this is common to all of them
        [RequiredArgument(0, nameof(common1), "Required parameter 1")]
        public string common1 { get; set; }

        [CommonArgument] // this is common to all of them
        [RequiredArgument(1, nameof(common2), "Required parameter 2")]
        public string common2 { get; set; }

        [ArgumentGroup(nameof(Command.Command1))]
        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public int opt1 { get; set; }

        [ArgumentGroup(nameof(Command.Command2))]
        [RequiredArgument(2, "req3", "Required parameter 3 (specific to command2)")]
        public Enum1 req3 { get; set; }

        [ArgumentGroup(nameof(Command.Command2))]
        [OptionalArgument(256, "opt2", "Optional parameter 1")]
        public int opt2 { get; set; }
    }
}
