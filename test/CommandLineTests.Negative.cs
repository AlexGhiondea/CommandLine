using CommandLine.ColorScheme;
using CommandLine.Tests.TestObjects;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace CommandLine.Tests
{
    public partial class CommandLineTests
    {
        [Trait("Category", "Negative")]
        [Fact]
        public void NegativeTest1()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<OptionsNegative_BadDefaultValue>("-opt2 b"));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void NegativeTest2()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<OptionsNegative1>("a"));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void NegativeTest3()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<OptionsNegative1>(""));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void NegativeTest4()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<OptionsNegative2>(""));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void NegativeTest5()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<OptionsNegative2_multipleReqSamePos>(""));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void NegativeTest6()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<OptionsNegative2_multipleOptSameName>(""));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void NegativeTest7()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<OptionsNegative2_CommandMultiple>(""));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void ExtraParameters1()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Options2>("a b -opt1 1 -opt2 b -opt3 c -opt4 t"));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void OptionalParameterDoesNotStartWithMinus()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Options1>("a 1 opt1 a"));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void TypeHasGroupsButDoesNotDefineCommand()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Groups_NoCommand>("a b opt1 a"));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void NotEnoughRequiredParametersSpecified()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Options1>("a "));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void NotEnoughRequiredParametersSpecifiedForACommand()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Groups1>("Command1"));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void UnknownOptionalArgument()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Options1>("a 1 -doesNotExist 43"));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void ParseUnknownCommand()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Groups1>("a"));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void ParseTypeWithWrongRequiredParamPosition()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<RequiredNegative_InvalidPositionForRequiredArg>(""));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void HelpForInvalidType()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<object>(""));
            Assert.IsType<ArgumentException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void MismatchedQuotes()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<object>("@\" foo "));
            Assert.IsType<FileNotFoundException>(exception.InnerException);
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void TryParseWithLoggingToConsole()
        {
            TestWriter _printer = new TestWriter();
            ParserOptions parserOptions = new ParserOptions() { LogParseErrorToConsole = true };
            IColors color = Parser.ColorScheme.Get();
            OutputColorizer.Colorizer.SetupWriter(_printer);
            bool value = Parser.TryParse("foo", out Options1 options, parserOptions);
            Assert.False(value);

            Validate(_printer,
                new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, $": Not all required arguments have been specified {Environment.NewLine}"),
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
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, "testhost --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
            );
        }

        [Trait("Category", "Negative")]
        [Fact]
        public void TryParseWithoutLoggingToConsole()
        {
            TestWriter _printer = new TestWriter();
            ParserOptions parserOptions = new ParserOptions() { LogParseErrorToConsole = false };
            IColors color = Parser.ColorScheme.Get();
            OutputColorizer.Colorizer.SetupWriter(_printer);
            bool value = Parser.TryParse("foo", out Options1 options, parserOptions);
            Assert.False(value);

            Validate(_printer);
        }


        [Trait("Category", "Negative")]
        [Fact]
        public void TryParseObjectThatDefinesTwoRequiredLists()
        {
            var commandLine = $"element1 element2 element3";

            TestWriter _printer = new TestWriter();
            IColors color = Parser.ColorScheme.Get();
            OutputColorizer.Colorizer.SetupWriter(_printer);

            bool value = Parser.TryParse(commandLine, out ComplexType2 options);
            Assert.False(value);

            Validate(_printer,
                new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, $": Cannot declare two required collection arguments. Please convert one of them to an optional collection one. {Environment.NewLine}"),
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, $"{Assembly.GetEntryAssembly()?.GetName()?.Name}.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "repos"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, "testhost --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
                );
        }

        [Trait("Category", "Collections")]
        [Fact]
        public void TryParseObjectThatHasListAsFirstArgument()
        {
            var commandLine = $"element1 element2 element3";

            TestWriter _printer = new TestWriter();
            IColors color = Parser.ColorScheme.Get();
            OutputColorizer.Colorizer.SetupWriter(_printer);

            bool value = Parser.TryParse(commandLine, out ComplexType5 options);
            Assert.False(value);

            Validate(_printer, new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, $": The required collection argument needs to be on the last position of the required arguments. {Environment.NewLine}"), 
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "list1"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "nonList"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, "testhost --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
                );
        }


        [Trait("Category", "Collections")]
        [Fact]
        public void DefineTwoRequiredCollectionsSameGroup()
        {
            string expectedRepo1 = "azure\azure-powershell";
            string expectedRepo2 = "azure\azure-sdk-for-net";
            var commandLine = $"Create {expectedRepo1} {expectedRepo2}";

            TestWriter _printer = new TestWriter();
            IColors color = Parser.ColorScheme.Get();
            OutputColorizer.Colorizer.SetupWriter(_printer);

            bool value = Parser.TryParse(commandLine, out ComplexType6 options);
            Assert.False(value);

            Validate(_printer, new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, $": Cannot declare two required collection arguments. Please convert one of them to an optional collection one. {Environment.NewLine}"), 
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentValueColor, "Create"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "repos2"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentValueColor, "List"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, "testhost --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
                );
        }


        [Trait("Category", "Collections")]
        [Fact]
        public void DefineRequiredCollectionWrongPositionGroup()
        {
            string expectedRepo1 = "azure\azure-powershell";
            string expectedRepo2 = "azure\azure-sdk-for-net";
            var commandLine = $"Create {expectedRepo1} {expectedRepo2}";

            TestWriter _printer = new TestWriter();
            IColors color = Parser.ColorScheme.Get();
            OutputColorizer.Colorizer.SetupWriter(_printer);

            bool value = Parser.TryParse(commandLine, out ComplexType7 options);
            Assert.False(value);

            Validate(_printer, new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, $": The required collection argument for group `Create` needs to be on the last position of the required arguments. {Environment.NewLine}"), 
                new TextAndColor(_printer.ForegroundColor, "Usage: "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentValueColor, "Create"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "repos"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "objectsFile"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "token"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentValueColor, "CreateOrUpdate"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "repos"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "objectsFile"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "token"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentValueColor, "List"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "repos"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, $"{Environment.NewLine}"), 
                new TextAndColor(color.ErrorColor, "Error"),
                new TextAndColor(_printer.ForegroundColor, ": Required argument expected at position"),
                new TextAndColor(color.RequiredArgumentColor, "1"),
                new TextAndColor(_printer.ForegroundColor, ". Type declares arguments at position(s) "),
                new TextAndColor(color.RequiredArgumentColor, "0,2"),
                new TextAndColor(_printer.ForegroundColor, ". "),
                new TextAndColor(color.ErrorColor, "Check type definition"),
                new TextAndColor(_printer.ForegroundColor, "."),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.ArgumentValueColor, "Check"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "repos"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(color.RequiredArgumentColor, "objectsFile"),
                new TextAndColor(_printer.ForegroundColor, " "),
                new TextAndColor(_printer.ForegroundColor, "[-"),
                new TextAndColor(color.OptionalArgumentColor, "token"),
                new TextAndColor(_printer.ForegroundColor, " value] "),
                new TextAndColor(_printer.ForegroundColor, "For detailed information run '"),
                new TextAndColor(color.AssemblyNameColor, "testhost --help"),
                new TextAndColor(_printer.ForegroundColor, "'.")
                );
        }
    }
}