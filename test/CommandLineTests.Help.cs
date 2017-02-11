using System;
using Xunit;

namespace CommandLine.Tests
{
    public partial class CommandLineTests
    {
        private void Validate(TestWriter _printer, params TextAndColor[] values)
        {
            Assert.Equal(values.Length, _printer.Segments.Count);

            for (int i = 0; i < values.Length; i++)
            {
                Assert.True(values[i].Equals(_printer.Segments[i]),
                    string.Format("Expected Text={0}, Color={1}, Got {2}", values[i].Text, values[i].Color, _printer.Segments[i]));
            }

        }

        [Fact]
        public void HelpTest1()
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>("-?", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "For detailed information run '"),
                new TextAndColor(ConsoleColor.White, "testhost --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
            );
        }

        [Fact]
        public void HelpTest4()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Options3NoRequired>("/?", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "For detailed information run '"),
                new TextAndColor(ConsoleColor.White, "testhost --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
            );
        }

        [Fact]
        public void HelpTest2()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Options3NoRequired>("-?", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "For detailed information run '"),
                new TextAndColor(ConsoleColor.White, "testhost --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
            );

        }

        [Fact]
        public void HelpTest3()
        {
            TestWriter _printer = new TestWriter();
            
            var options = Helpers.Parse<OptionsNegative1>("-?", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p2"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "For detailed information run '"),
                new TextAndColor(ConsoleColor.White, "testhost --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
            );
        }

        [Fact]
        public void DetailedHelp1()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Options1>("--help", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p2"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p1  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 "),
                new TextAndColor(ConsoleColor.Magenta, "(required)"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p2  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 2 "),
                new TextAndColor(ConsoleColor.Magenta, "(required)"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 (default="),
                new TextAndColor(ConsoleColor.Green, "256"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 2 (default="),
                new TextAndColor(ConsoleColor.Green, "all"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 2 (default="),
                new TextAndColor(ConsoleColor.Black, ")")
            );
        }

        [Fact]
        public void DetailedHelpForGroups1()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Groups1>("--help", _printer);
            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Green, "Command1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p1  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 "),
                new TextAndColor(ConsoleColor.Magenta, "(required)"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 (default="),
                new TextAndColor(ConsoleColor.Green, "256"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Green, "Command2"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "opt1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 (one of "),
                new TextAndColor(ConsoleColor.Green, "A,B"),
                new TextAndColor(ConsoleColor.Black, ") "),
                new TextAndColor(ConsoleColor.Magenta, "(required)"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 (one of "),
                new TextAndColor(ConsoleColor.Cyan, "A,B"),
                new TextAndColor(ConsoleColor.Black, ") (default="),
                new TextAndColor(ConsoleColor.Green, "256"),
                new TextAndColor(ConsoleColor.Black, ")")
            );
        }

        [Fact]
        public void HelpForCommand()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Groups1>("Command1 -?", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Green, "Command1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p1  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 "),
                new TextAndColor(ConsoleColor.Magenta, "(required)"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 (default="),
                new TextAndColor(ConsoleColor.Green, "256"),
                new TextAndColor(ConsoleColor.Black, ")")
            );
        }

        [Fact]
        public void HelpForCommandWithSlashQuestionMark()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Groups1>("Command1 /?", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Green, "Command1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p1  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 "),
                new TextAndColor(ConsoleColor.Magenta, "(required)"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 (default="),
                new TextAndColor(ConsoleColor.Green, "256"),
                new TextAndColor(ConsoleColor.Black, ")")
            );
        }

        [Fact]
        public void HelpForTypeWithEnum()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Options3NoRequired>("/?", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "For detailed information run '"),
                new TextAndColor(ConsoleColor.White, "testhost --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
            );
        }
    }
}