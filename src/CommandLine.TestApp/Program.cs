using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
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
            args = "a 1 -opt1 3 -opt2 3 -opt3 c -opt4 t".Split(' ');
            var x111 = CommandLine.Parser.Parse<Options1>(args);

            args = new string[] { "-?" };
            var x = CommandLine.Parser.Parse<Groups1>(args);
            Console.WriteLine("----------------");
            var x1 = CommandLine.Parser.Parse<Options1>(args);
            Console.WriteLine("============================");
            args = new string[] { "--help" };
            var y = CommandLine.Parser.Parse<Groups1>(args);
            Console.WriteLine("----------------");
            var y2 = CommandLine.Parser.Parse<Options1>(args);

            args = new string[] { "Command1", "-?" };
            Console.WriteLine("============================");
            var x3 = CommandLine.Parser.Parse<Groups1>(args);

            args = new string[] { "NoCommands", "-?" };
            Console.WriteLine("============================");
            var x4 = CommandLine.Parser.Parse<Groups1>(args);

            args = new string[] { "NoCommands" };
            Console.WriteLine("============================");
            var x44 = CommandLine.Parser.Parse<Groups1>(args);

            args = new string[] { "Command1", };
            Console.WriteLine("============================");
            var x32 = CommandLine.Parser.Parse<Groups1>(args);

            args = new string[] { "--help" };
            Console.WriteLine("============================");
            var x5 = CommandLine.Parser.Parse<Groups1>(args);
            Console.WriteLine("----------------");
            var y4 = CommandLine.Parser.Parse<Options1>(args);


            args = new string[] { };
            Console.WriteLine("============================");
            var x55 = CommandLine.Parser.Parse<Groups1>(args);

            Console.WriteLine("----------------");
            var y455 = CommandLine.Parser.Parse<Options1>(args);
        }
    }

    internal enum Command
    {
        Command2 = 2,
        Command1 = 1,
        NoCommands = 3
    }
    internal class Groups1
    {
        [ActionArgument]
        public Command Command { get; set; }

        [ArgumentGroup(nameof(Command.Command1))]
        [ArgumentGroup(nameof(Command.Command2))]
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public string p1 { get; set; }

        [ArgumentGroup(nameof(Command.Command1))]
        [OptionalArgument(256, "opt1", "Optional parameter 1")]
        public int opt1 { get; set; }

        [ArgumentGroup(nameof(Command.Command2))]
        [RequiredArgument(1, "p2", "Required parameter 1")]
        public string p2 { get; set; }

        [ArgumentGroup(nameof(Command.Command2))]
        [RequiredArgument(2, "p21212", "Required parameter 1")]
        public string p3 { get; set; }

        [ArgumentGroup(nameof(Command.Command2))]
        [OptionalArgument("aa", "opt1", "Optional parameter 1")]
        public int opt2 { get; set; }
    }
}
