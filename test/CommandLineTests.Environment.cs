using System;
using Xunit;

namespace CommandLine.Tests
{
    public partial class CommandLineTests
    {
        [Fact]
        public void EnvironmentTest1()
        {
            ParserOptions po = new ParserOptions() { VariableNamePrefix = nameof(EnvironmentTest1) };

            Environment.SetEnvironmentVariable($"{nameof(EnvironmentTest1)}opt1", "10", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable($"{nameof(EnvironmentTest1)}opt3", "A", EnvironmentVariableTarget.Process);

            var options = Helpers.Parse<Options3NoRequired>("-opt2 d e fc", null, null, po);

            Assert.Equal(10, options.opt1);
            Helpers.CollectionEquals(options.opt2, "d", "e", "fc");
            Assert.Equal(Enum1.A, options.opt3);

            Environment.SetEnvironmentVariable($"{nameof(EnvironmentTest1)}opt1", "", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable($"{nameof(EnvironmentTest1)}opt2", "", EnvironmentVariableTarget.Process);
        }
    }
}