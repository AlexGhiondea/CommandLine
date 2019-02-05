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

        [Fact]
        public void GroupsTest4()
        {
            var options = Helpers.Parse<OverridePositionGroup>("Create MileStoneFile Repo1 Repo2");

            Assert.Equal(Action.Create, options.Action);
            Assert.Equal("MileStoneFile", options.MilestoneFile);
            Assert.Equal(2, options.Repositories.Count);
            Assert.Equal("Repo1", options.Repositories[0]);
            Assert.Equal("Repo2", options.Repositories[1]);

            options = Helpers.Parse<OverridePositionGroup>("List Repo1 Repo2");

            Assert.NotNull(options);
            Assert.Equal(Action.List, options.Action);
            Assert.Equal(2, options.Repositories.Count);
            Assert.Equal("Repo1", options.Repositories[0]);
            Assert.Equal("Repo2", options.Repositories[1]);
        }

        [Fact]
        public void GroupsTest5()
        {
            // this should not return a good parsed object
            var options = Helpers.Parse<OverridePositionGroup_Conflict>("List MileStoneFile Repo1 Repo2");

            Assert.Null(options);
        }

        [Fact]
        public void GroupsTest6()
        {
            var options = Helpers.Parse<OverridePositionGroup2>("Create Repo1 MileStoneFile");
           
            Assert.Equal(Action.Create, options.Action);
            Assert.Equal("MileStoneFile", options.MilestoneFile);
            Assert.Equal("Repo1", options.Repository);

            options = Helpers.Parse<OverridePositionGroup2>("List MileStoneFile Repo1");

            Assert.Equal(Action.List, options.Action);
            Assert.Equal("MileStoneFile", options.MilestoneFile);
            Assert.Equal("Repo1", options.Repository);
        }
    }
}