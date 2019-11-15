using CommandLine.ColorScheme;
using System;
using System.IO;
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
                new TextAndColor(color.AssemblyNameColor, "testhost.exe"),
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
    }
}