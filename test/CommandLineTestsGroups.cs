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

        [Fact]
        public void GroupsTest2()
        {
            var options = Helpers.Parse<Groups2>("Command1 common1 common2 -opt1 10");

            Assert.Equal("common1", options.common1);
            Assert.Equal("common2", options.common2);
            Assert.Equal(10, options.opt1);
        }

        [Fact]
        public void GroupsTest3()
        {
            var options = Helpers.Parse<Groups2>("Command2 common1 common2 A -opt2 200");

            Assert.Equal("common1", options.common1);
            Assert.Equal("common2", options.common2);
            Assert.Equal(Enum1.A, options.req3);
            Assert.Equal(200, options.opt2);
        }

    }
}