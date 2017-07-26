using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace CommandLine.Tests
{
    public partial class CommandLineTests
    {
        [Fact]
        public void GroupsTest1_WithResponseFile()
        {
            string responseFile = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles","response1.rsp"));

            var options = Helpers.Parse<Groups1>($"@{responseFile}");

            Assert.Equal("p1", options.p1);
            Assert.Equal(10, options.opt1);
        }

        [Fact]
        public void BasicTest5_WithResponseFile()
        {
            string responseFile1 = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles","response2_1.rsp"));
            string responseFile2 = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles","response2_2.rsp"));

            var options = Helpers.Parse<Options1>($"@{responseFile1} @{responseFile2}");

            Assert.Equal("p1", options.p1);
            Assert.Equal(2, options.p2);
            Assert.Equal(10, options.opt1);
            Assert.Equal("all", options.opt2);
            Helpers.CollectionEquals(options.opt3, "a", "b", "c");
        }

        [Fact]
        public void BasicTest7_WithResponseFile()
        {
            string responseFile1 = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles","response3_1.rsp"));
            string responseFile2 = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles","response3_2.rsp"));

            var options = Helpers.Parse<Options2>($"@{responseFile1} -opt2 a b c @{responseFile2}");

            Assert.Equal("p1", options.p1);
            Helpers.CollectionEquals(options.p2, "d", "e", "fc");
            Assert.Equal(10, options.opt1);
            Helpers.CollectionEquals(options.opt2, "a", "b", "c");
        }

        [Fact]
        public void HelpTest1_WithResponseFile()
        {
            string responseFile = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles","response4.rsp"));

            TestWriter _printer = new TestWriter();
            var options = Helpers.Parse<Options3NoRequired>($"@{responseFile}", _printer);

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
        public void ResponseFileEscaping()
        {
            string responseFile = Path.Combine(Helpers.GetTestLocation(), Path.Combine("SampleRspFiles","response5.rsp"));

            var options = Helpers.Parse<Options2>($"@{responseFile}");

            Assert.Equal("p1", options.p1);
            Helpers.CollectionEquals(options.p2, "this is d", "very", "interesting to see");
            Assert.Equal(10, options.opt1);
            Helpers.CollectionEquals(options.opt2, "a", "b", "c\t d");
        }
    }
}
