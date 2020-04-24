using CommandLine.Tests.TestObjects;
using System;
using Xunit;

namespace CommandLine.Tests
{
    public partial class CommandLineTests
    {
        [Trait("Category", "Groups")]
        [Fact]
        public void GroupsTest1()
        {
            var options = Helpers.Parse<Groups1>("Command1 p1 -opt1 10");

            Assert.Equal("p1", options.p1);
            Assert.Equal(10, options.opt1);
        }

        [Trait("Category", "Groups")]
        [Fact]
        public void GroupsTest2()
        {
            var options = Helpers.Parse<Groups2>("Command1 common1 common2 -opt1 10");

            Assert.Equal("common1", options.common1);
            Assert.Equal("common2", options.common2);
            Assert.Equal(10, options.opt1);
        }

        [Trait("Category", "Groups")]
        [Fact]
        public void GroupsTest3()
        {
            var options = Helpers.Parse<Groups2>("Command2 common1 common2 A -opt2 200");

            Assert.Equal("common1", options.common1);
            Assert.Equal("common2", options.common2);
            Assert.Equal(Enum1.A, options.req3);
            Assert.Equal(200, options.opt2);
        }

        [Trait("Category", "Groups")]
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

        [Trait("Category", "Groups")]
        [Fact]
        public void GroupsTest7()
        {
            var options = Helpers.Parse<OverridePositionGroupWithMoreArgs>("Create One Two Three");

            Assert.Equal(Action.Create, options.Action);
            Assert.Equal("One", options.One);
            Assert.Equal("Two", options.Two);
            Assert.Equal("Three", options.Three);

            options = Helpers.Parse<OverridePositionGroupWithMoreArgs>("List One Two Three");

            Assert.NotNull(options);
            Assert.Equal(Action.List, options.Action);
            Assert.Equal("Two", options.One);
            Assert.Equal("Two", options.One);
            Assert.Equal("Three", options.Three);
        }

        [Trait("Category", "Groups")]
        [Fact]
        public void GroupsTest5()
        {
            var exception = Assert.Throws<ParserException>(() => Helpers.Parse<OverridePositionGroup_Conflict>("List MileStoneFile Repo1 Repo2"));
        }

        [Trait("Category", "Groups")]
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

        [Trait("Category", "Groups")]
        [Fact]
        public void GroupsTest8()
        {
            // Invalid option should be in exception text
            var commandLine = "Command1 req1 -opt1=value";

            Groups1 ParsedArgs;
            Assert.False(Parser.TryParse(commandLine, out ParsedArgs));
        }

        [Trait("Category", "Groups")]
        [Fact]
        public void TestRequiredListBeforeOptionalArg()
        {
            string expectedRepo1 = "azure\azure-powershell";
            string expectedRepo2 = "azure\azure-sdk-for-net";
            var commandLine = $"List Label,Milestone {expectedRepo1} {expectedRepo2} -token <mySecretValue>";

            Assert.True(Parser.TryParse(commandLine, out ComplexType1 complex));

            Assert.Equal(CommandAction.List, complex.Action);
            Assert.Equal(ObjectType.Label | ObjectType.Milestone, complex.ObjectType);
            Assert.Equal(2, complex.Repositories.Count);
            Helpers.CollectionEquals(complex.Repositories, expectedRepo1, expectedRepo2);
        }

        [Trait("Category", "Collections")]
        [Fact]
        public void TestRequiredPropertiesOnDifferentGroups()
        {
            string expectedRepo1 = "azure\azure-powershell";
            string expectedRepo2 = "azure\azure-sdk-for-net";
            var commandLine = $"Check {expectedRepo1} {expectedRepo2}";

            Assert.True(Parser.TryParse(commandLine, out ComplexType4 complex));
            Assert.Equal(CommandAction.Check, complex.Action);
            Helpers.CollectionEquals(complex.List1, expectedRepo1, expectedRepo2);

            commandLine = $"List {expectedRepo1} {expectedRepo2}";

            Assert.True(Parser.TryParse(commandLine, out complex));
            Assert.Equal(CommandAction.List, complex.Action);
            Helpers.CollectionEquals(complex.List2, expectedRepo1, expectedRepo2);
        }

        [Trait("Category", "Collections")]
        [Fact]
        public void TestObjectWithJustList()
        {
            string expectedRepo1 = "azure\azure-powershell";
            string expectedRepo2 = "azure\azure-sdk-for-net";
            var commandLine = $"{expectedRepo1} {expectedRepo2}";

            Assert.True(Parser.TryParse(commandLine, out SimpleType1 complex));
            Helpers.CollectionEquals(complex.List1, expectedRepo1, expectedRepo2);
        }
    }
}
