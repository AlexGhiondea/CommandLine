using System;
using Xunit;

namespace CommandLine.Tests
{
    
    public class Groups
    {
        [Fact]
        public void GroupsTest1()
        {
            var options = Helpers.Parse<Groups1>("Command1 p1 -opt1 10");

            Assert.Equal(options.p1, "p1");
            Assert.Equal(options.opt1, 10);
        }
   }
}