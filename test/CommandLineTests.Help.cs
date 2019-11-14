using System;
using System.Collections.Generic;
using System.Reflection;
using CommandLine.ColorScheme;
using Xunit;
using Xunit.Extensions;

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

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpTest1(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>("-?", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpTestViaApi1(IColors color)
        {
            TestWriter _printer = new TestWriter();
            Helpers.DisplayHelp<Options3NoRequired>(HelpFormat.Short, _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpTest4(IColors color)
        {
            TestWriter _printer = new TestWriter();

            var options = Helpers.Parse<Options3NoRequired>("/?", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpTest2(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>("-?", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );

        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpTest3(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<OptionsNegative1>("-?", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p2"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void DetailedHelp1(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options1>("--help", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p2"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt4"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "p1  "),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "p2  "),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "all"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "list"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt4"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 4 ("),
                new TextAndColor(color.ArgumentValueColor, "char"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "z"),
                new TextAndColor(_printer.ForegroundColor, ")")
              );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void DetailedHelp2(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>("--help", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "list"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 3 (one of "),
                new TextAndColor(color.ArgumentValueColor, "A,B"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "B"),
                new TextAndColor(_printer.ForegroundColor, ")")
                );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void DetailedHelpViaApi1(IColors color)
        {
            TestWriter _printer = new TestWriter();
            Helpers.DisplayHelp<Options1>(HelpFormat.Full, _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p2"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt4"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "p1  "),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "p2  "),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "all"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "list"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt4"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 4 ("),
                new TextAndColor(color.ArgumentValueColor, "char"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "z"),
                new TextAndColor(_printer.ForegroundColor, ")")
              );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void DetailedHelpForGroups1(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Groups1>("--help", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentGroupColor, "Command1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "p1  "),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentGroupColor, "Command2"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 (one of "),
                new TextAndColor(color.ArgumentValueColor, "A,B"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 (one of "),
                new TextAndColor(color.ArgumentValueColor, "A,B"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")")
                 );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpForCommand(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Groups1>("Command1 -?", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentGroupColor, "Command1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "p1  "),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpForCommandWithSlashQuestionMark(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Groups1>("Command1 /?", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentGroupColor, "Command1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "p1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "p1  "),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpForTypeWithEnum(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>("/?", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpForTypeWithEnumWithNoHelpSettingSet(IColors color)
        {
            // the help is not covered by the error flag.
            ParserOptions po = new ParserOptions() { LogParseErrorToConsole = false };
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>("/?", _printer, color, po);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, "testhost --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );

            /*
             ,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt3"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name} --help"),
                new TextAndColor(_printer.ForegroundColor, "'."
             */
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void DetailedHelpForGroups2WithCommonArgs(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Groups2>("--help", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentGroupColor, "Command1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "common1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "common2"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt1"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "common1"),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "common2"),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt1   "),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentGroupColor, "Command2"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "common1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "common2"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "req3"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "opt2"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "common1"),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "common2"),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 2 ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "req3   "),
                new TextAndColor(_printer.ForegroundColor, " : Required parameter 3 (specific to command2) (one of "),
                new TextAndColor(color.ArgumentValueColor, "A,B"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "opt2   "),
                new TextAndColor(_printer.ForegroundColor, " : Optional parameter 1 ("),
                new TextAndColor(color.ArgumentValueColor, "number"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "256"),
                new TextAndColor(_printer.ForegroundColor, ")")
            );
        }

        [Trait("Category", "Help")]
        [Fact]
        public void InvalidHelpFormat()
        {
            var exception = Assert.Throws<ParserException>(() => Parser.DisplayHelp<Options1>((HelpFormat)4));
            Assert.Null(exception.InnerException);
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpForTypeWithRequiredAndOptionalEnumsAndLists(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<HelpGeneratorObject>("--help", _printer, color);

            Validate(_printer,
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "folders"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "providers"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "provider"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "out"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "open"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "outputWriter"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "folders     "),
                new TextAndColor(_printer.ForegroundColor, " : List of the folders to consider when scanning for duplicates ("),
                new TextAndColor(color.ArgumentValueColor, "list"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.RequiredArgumentColor, "providers   "),
                new TextAndColor(_printer.ForegroundColor, " : Some providers to have (one of "),
                new TextAndColor(color.ArgumentValueColor, "SHA1,FileSize"),
                new TextAndColor(_printer.ForegroundColor, ", "),
                new TextAndColor(color.RequiredArgumentColor, "required"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "provider    "),
                new TextAndColor(_printer.ForegroundColor, " : The mechanism to use when determining if the files are unique (one of "),
                new TextAndColor(color.ArgumentValueColor, "SHA1,FileSize"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "FileSize"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "out         "),
                new TextAndColor(_printer.ForegroundColor, " : The name of the file where to write the result ("),
                new TextAndColor(color.ArgumentValueColor, "string"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "output"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "open        "),
                new TextAndColor(_printer.ForegroundColor, " : Launch the result once the tool runs ("),
                new TextAndColor(color.ArgumentValueColor, "true or false"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "True"),
                new TextAndColor(_printer.ForegroundColor, ")"),
                new TextAndColor(_printer.ForegroundColor, "  - "),
                new TextAndColor(color.OptionalArgumentColor, "outputWriter"),
                new TextAndColor(_printer.ForegroundColor, " : The output format(s) to use (one of "),
                new TextAndColor(color.ArgumentValueColor, "Html,Csv"),
                new TextAndColor(_printer.ForegroundColor, ", default="),
                new TextAndColor(color.OptionalArgumentColor, "Html, Csv"),
                new TextAndColor(_printer.ForegroundColor, ")")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpWhenPassMoreParametersThanExpected(IColors color)
        {
            TestWriter _printer = new TestWriter();
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<MorePassedInThanRequired>("this expects 2 args", _printer, color));
            Assert.IsType<ArgumentException>(exception.InnerException);

            Validate(_printer,
                new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, $": Optional parameter name should start with '-' {Environment.NewLine}"),
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "a"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "b"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, "testhost --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void ParseForBadType(IColors color)
        {
            TestWriter _printer = new TestWriter();

            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<TestWriter>("this expects 2 args", _printer, color));
            Assert.IsType<ArgumentException>(exception.InnerException);

            Validate(_printer,
                new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, $": Cannot have groups unless Command argument has been specified {Environment.NewLine}"),
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, "testhost --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
                );
        }

        [Trait("Category", "Help")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpForBadType(IColors color)
        {
            TestWriter _printer = new TestWriter();
            Helpers.DisplayHelp<OptionsNegative2>(HelpFormat.Full, _printer, color);
            Validate(_printer,
                new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, $": Only one of Required/Optional attribute are allowed per property (p1). Help information might be incorrect! {Environment.NewLine}")
            );
        }

        [Trait("Category", "Color")]
        [Fact]
        public void GetColorScheme()
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            try
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get());
            }
            finally
            {
                Console.BackgroundColor = currentBackgroundColor;
            }
        }

        [Trait("Category", "Color")]
        [Fact]
        public void ValidateBackgroundColorScheme()
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            try
            {
                #region Dark backgound
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.Black));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.DarkGray));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.DarkCyan));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.DarkGreen));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.DarkMagenta));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.DarkRed));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.Blue));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.Magenta));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.DarkBlue));
                #endregion

                #region Multicolor
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<GrayBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.Gray));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkYellowBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.DarkYellow));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<GreenBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.Green));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<CyanBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.Cyan));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<RedBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.Red));

                #endregion

                #region Light background
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<LightBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.White));
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<LightBackground>(Parser.Configuration.ColorScheme.Get(ConsoleColor.Yellow));
                #endregion

                #region Unknown color
                Parser.Configuration.ColorScheme.Set(null);
                Assert.IsType<DarkBackground>(Parser.Configuration.ColorScheme.Get((ConsoleColor)25));
                #endregion
            }
            finally
            {
                Console.BackgroundColor = currentBackgroundColor;
            }
        }

        public static IEnumerable<object[]> GetBackgroundColors()
        {
            yield return new object[] { new DarkBackground() };
            yield return new object[] { new LightBackground() };
            yield return new object[] { new DarkYellowBackground() };
            yield return new object[] { new GreenBackground() };
            yield return new object[] { new CyanBackground() };
            yield return new object[] { new RedBackground() };
            yield return new object[] { new GrayBackground() };
        }

        [Trait("Category", "Help")]
        [Fact]
        public void EnsureThatRightParameterIsReportedForGroups()
        {
            // Invalid option should be in exception text
            var commandLine = "Command1 req1 -opt1=value";

            TestWriter _printer = new TestWriter();

            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Groups1>(commandLine, _printer));
            Assert.IsType<ArgumentException>(exception.InnerException);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Red, "Error"),
                new TextAndColor(ConsoleColor.Black, $": Could not find argument -opt1=value {Environment.NewLine}"),
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
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

        [Trait("Category", "Help")]
        [Fact]
        public void ErrorWhenNoRequiredParametersInGroupSpecified()
        {
            // Invalid option should be in exception text
            var commandLine = "";

            TestWriter _printer = new TestWriter();

            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Groups1>(commandLine, _printer));
            Assert.IsType<ArgumentException>(exception.InnerException);

            Validate(_printer,
                new TextAndColor(ConsoleColor.Red, "Error"),
                new TextAndColor(ConsoleColor.Black, $": Required parameters have not been specified {Environment.NewLine}"),
                new TextAndColor(ConsoleColor.Black, "Usage: "),
                new TextAndColor(ConsoleColor.Black, " "),
                new TextAndColor(ConsoleColor.White, "testhost.exe"),
                new TextAndColor(ConsoleColor.Black, " "),
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