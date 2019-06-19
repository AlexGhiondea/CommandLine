using System;
using System.Reflection;
using Xunit;

namespace CommandLine.Tests
{
    public partial class CommandLineTests
    {
        private void Validate(TestWriter _printer, params TextAndColor[] values)
        {
            Assert.True(values.Length == _printer.Segments.Count, _printer.ToTestCode());

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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
            );
        }

        [Fact]
        public void HelpTestViaApi1()
        {
            TestWriter _printer = new TestWriter();
            Helpers.DisplayHelp<Options3NoRequired>(HelpFormat.Short, _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "p2"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "For detailed information run '"),
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt4"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p1  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p2  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "all"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "list"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt4"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 4 ("),
                new TextAndColor(ConsoleColor.Green, "char"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "z"),
                new TextAndColor(ConsoleColor.Black, ")")
              );
        }

        [Fact]
        public void DetailedHelp2()
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>("--help", _printer);

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
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "list"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 3 (one of "),
                new TextAndColor(ConsoleColor.Green, "A,B"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "B"),
                new TextAndColor(ConsoleColor.Black, ")")
                );
        }


        [Fact]
        public void DetailedHelpViaApi1()
        {
            TestWriter _printer = new TestWriter();
            Helpers.DisplayHelp<Options1>(HelpFormat.Full, _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt4"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p1  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "p2  "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "all"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt3"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "list"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt4"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 4 ("),
                new TextAndColor(ConsoleColor.Green, "char"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "z"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 (one of "),
                new TextAndColor(ConsoleColor.Green, "A,B"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
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
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
            );
        }

        [Fact]
        public void DetailedHelpForGroups2WithCommonArgs()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Groups2>("--help", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Green, "Command1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "common1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "common2"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt1"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "common1"),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "common2"),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt1   "),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Green, "Command2"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "common1"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "common2"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "req3"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "opt2"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "common1"),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "common2"),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 2 ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "req3   "),
                new TextAndColor(ConsoleColor.Black, " : Required parameter 3 (specific to command2) (one of "),
                new TextAndColor(ConsoleColor.Green, "A,B"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "opt2   "),
                new TextAndColor(ConsoleColor.Black, " : Optional parameter 1 ("),
                new TextAndColor(ConsoleColor.Green, "number"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "256"),
                new TextAndColor(ConsoleColor.Black, ")")
            );
        }

        [Fact]
        public void InvalidHelpFormat()
        {
            Assert.Throws<ArgumentException>(() => Parser.DisplayHelp<Options1>((HelpFormat)4));
        }

        [Fact]
        public void HelpForTypeWithRequiredAndOptionalEnumsAndLists()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<HelpGeneratorObject>("--help", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "folders"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "providers"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "provider"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "out"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "open"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "[-"),
                new TextAndColor(ConsoleColor.Yellow, "outputWriter"),
                new TextAndColor(ConsoleColor.Black, " value] "),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "folders     "),
                new TextAndColor(ConsoleColor.Black, " : List of the folders to consider when scanning for duplicates ("),
                new TextAndColor(ConsoleColor.Green, "list"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Cyan, "providers   "),
                new TextAndColor(ConsoleColor.Black, " : Some providers to have (one of "),
                new TextAndColor(ConsoleColor.Green, "SHA1,FileSize"),
                new TextAndColor(ConsoleColor.Black, ", "),
                new TextAndColor(ConsoleColor.Cyan, "required"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "provider    "),
                new TextAndColor(ConsoleColor.Black, " : The mechanism to use when determining if the files are unique (one of "),
                new TextAndColor(ConsoleColor.Green, "SHA1,FileSize"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "FileSize"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "out         "),
                new TextAndColor(ConsoleColor.Black, " : The name of the file where to write the result ("),
                new TextAndColor(ConsoleColor.Green, "string"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "output"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "open        "),
                new TextAndColor(ConsoleColor.Black, " : Launch the result once the tool runs ("),
                new TextAndColor(ConsoleColor.Green, "true or false"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "True"),
                new TextAndColor(ConsoleColor.Black, ")"),
                new TextAndColor(ConsoleColor.Black, "  - "),
                new TextAndColor(ConsoleColor.Yellow, "outputWriter"),
                new TextAndColor(ConsoleColor.Black, " : The output format(s) to use (one of "),
                new TextAndColor(ConsoleColor.Green, "Html,Csv"),
                new TextAndColor(ConsoleColor.Black, ", default="),
                new TextAndColor(ConsoleColor.Yellow, "Html, Csv"),
                new TextAndColor(ConsoleColor.Black, ")")
            );
        }

        [Fact]
        public void HelpWhenPassMoreParametersThanExpected()
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<MorePassedInThanRequired>("this expects 2 args", _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Red, "Error"),
                new TextAndColor(ConsoleColor.Black, $": Optional parameter name should start with '-' {Environment.NewLine}"),
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "a"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Cyan, "b"),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.Black, "For detailed information run '"),
                new TextAndColor(ConsoleColor.White, "testhost --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
            );
        }

        [Fact]
        public void EnsureThatRightParameterIsReportedForGroups()
        {
            // Invalid option should be in exception text
            var commandLine = "Command1 req1 -opt1=value";

            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Groups1>(commandLine, _printer);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Red, "Error"),
                new TextAndColor(ConsoleColor.Black, $": Could not find argument -opt1=value {Environment.NewLine}"),
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
                new TextAndColor(ConsoleColor.Black, "For detailed information run '"),
                new TextAndColor(ConsoleColor.White, "testhost --help"),
                new TextAndColor(ConsoleColor.Black, "'.")
                );
        }
    }
}