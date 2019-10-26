using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using CommandLine.Colors;
using Xunit;

namespace CommandLine.Tests
{
    public partial class CommandLineTests
    {
        [Trait("Category", "Response file")]
        [Fact]
        public void GroupsTest1_WithResponseFile()
        {
            string responseFile = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles", "response1.rsp"));

            var options = Helpers.Parse<Groups1>($"@{responseFile}");

            Assert.Equal("p1", options.p1);
            Assert.Equal(10, options.opt1);
        }

        [Trait("Category", "Response file")]
        [Fact]
        public void TestResponseFileWithMismatchedQuotes()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<object>("@\" foo "));
            Assert.IsType<FileNotFoundException>(exception.InnerException);
        }

        [Trait("Category", "Response file")]
        [Fact]
        public void TestStringWithMismatchedQuotes()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<object>("\" foo "));
        }

        [Trait("Category", "Response file")]
        [Fact]
        public void BasicTest5_WithResponseFile()
        {
            string responseFile1 = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles", "response2_1.rsp"));
            string responseFile2 = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles", "response2_2.rsp"));

            var options = Helpers.Parse<Options1>($"@{responseFile1} @{responseFile2}");

            Assert.Equal("p1", options.p1);
            Assert.Equal(2, options.p2);
            Assert.Equal(10, options.opt1);
            Assert.Equal("all", options.opt2);
            Helpers.CollectionEquals(options.opt3, "a", "b", "c");
        }

        [Trait("Category", "Response file")]
        [Fact]
        public void BasicTest7_WithResponseFile()
        {
            string responseFile1 = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles", "response3_1.rsp"));
            string responseFile2 = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles", "response3_2.rsp"));

            var options = Helpers.Parse<Options2>($"@{responseFile1} -opt2 a b c @{responseFile2}");

            Assert.Equal("p1", options.p1);
            Helpers.CollectionEquals(options.p2, "d", "e", "fc");
            Assert.Equal(10, options.opt1);
            Helpers.CollectionEquals(options.opt2, "a", "b", "c");
        }

        [Trait("Category", "Response file")]
        [Theory, MemberData(nameof(GetBackgroundColors))]
        public void HelpTest1_WithResponseFile(IColors color)
        {
            string responseFile = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles", "response4.rsp"));

            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>($"@{responseFile}", _printer, color);

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

        [Trait("Category", "Response file")]
        [Fact]
        public void ResponseFileEscaping()
        {
            string responseFile = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles", "response5.rsp"));

            var options = Helpers.Parse<Options2>($"@{responseFile}");

            Assert.Equal("p1", options.p1);
            Helpers.CollectionEquals(options.p2, "this is d", "very", "interesting to see");
            Assert.Equal(10, options.opt1);
            Helpers.CollectionEquals(options.opt2, "a", "b", "c\t d");
        }

        [Trait("Category", "Response file")]
        [Fact]
        public void NotFoundResponseFile()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<Options2>("@doesNotExist"));
            Assert.IsType<FileNotFoundException>(exception.InnerException);
        }
    }
}
