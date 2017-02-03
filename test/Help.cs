using System;
using Xunit;

namespace CommandLine.Tests
{
    
    public class Help
    {

        [Fact]
        public void HelpTest1()
        {
            var options = Helpers.Parse<Options3NoRequired>("-?");
        }

        [Fact]
        public void HelpTest4()
        {
            var options = Helpers.Parse<Options3NoRequired>("/?");
        }

        [Fact]
        public void HelpTest2()
        {
            var options = Helpers.Parse<Options3NoRequired>("-?");
        }

        [Fact]
        public void HelpTest3()
        {
            var options = Helpers.Parse<OptionsNegative1>("-?");
        }

        [Fact]
        public void DetailedHelp1()
        {
            var options = Helpers.Parse<Options1>("--help");
        }

        [Fact]
        public void DetailedHelpForGroups1()
        {
            var options = Helpers.Parse<Groups1>("--help");
        }

        [Fact]
        public void HelpForCommand()
        {
            var options = Helpers.Parse<Groups1>("Command1 -?");
        }

        [Fact]
        public void HelpForCommandWithSlashQuestionMark()
        {
            var options = Helpers.Parse<Groups1>("Command1 /?");
        }

        [Fact]
        public void HelpForTypeWithEnum()
        {
            var options = Helpers.Parse<Options3NoRequired>("/?");
        }
    }
}