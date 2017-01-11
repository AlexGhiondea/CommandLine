using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLine.Tests
{
    [TestClass]
    public class Help
    {

        [TestMethod]
        public void HelpTest1()
        {
            var options = Helpers.Parse<Options3NoRequired>("-?");
        }

        [TestMethod]
        public void HelpTest4()
        {
            var options = Helpers.Parse<Options3NoRequired>("/?");
        }

        [TestMethod]
        public void HelpTest2()
        {
            var options = Helpers.Parse<Options3NoRequired>("-?");
        }

        [TestMethod]
        public void HelpTest3()
        {
            var options = Helpers.Parse<OptionsNegative1>("-?");
        }

        [TestMethod]
        public void DetailedHelp1()
        {
            var options = Helpers.Parse<Options1>("--help");
        }

        [TestMethod]
        public void DetailedHelpForGroups1()
        {
            var options = Helpers.Parse<Groups1>("--help");
        }

        [TestMethod]
        public void HelpForCommand()
        {
            var options = Helpers.Parse<Groups1>("Command1 -?");
        }

        [TestMethod]
        public void HelpForCommandWithSlashQuestionMark()
        {
            var options = Helpers.Parse<Groups1>("Command1 /?");
        }

        [TestMethod]
        public void HelpForTypeWithEnum()
        {
            var options = Helpers.Parse<Options3NoRequired>("/?");
        }
    }
}