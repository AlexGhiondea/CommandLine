using CommandLine.Attributes;
using CommandLine.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = CommandLine.Parser.Parse<Groups1>(args);
        }
    }

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

        [CommandGroupArgument(nameof(Command.Command2))]
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public string p2 { get; set; }

        [CommandGroupArgument(nameof(Command.Command2))]
        [RequiredArgument(1, "p21212", "Required parameter 1")]
        public string p3 { get; set; }

        [CommandGroupArgument(nameof(Command.Command2))]
        [OptionalArgument("aa", "opt1", "Optional parameter 1")]
        public int opt2 { get; set; }
    }
}
