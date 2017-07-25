using Xunit;

namespace CommandLine.Tests
{

    public partial class CommandLineTests
    {
        [Fact]
        public void GroupsTest1()
        {
            var options = Helpers.Parse<Groups1>("Command1 p1 -opt1 10");

            Assert.Equal("p1", options.p1);
            Assert.Equal(10, options.opt1);
        }
   }
}