using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLine.Tests
{
    [TestClass]
    public class Groups
    {
        [TestMethod]
        public void GroupsTest1()
        {
            var options = Helpers.Parse<Groups1>("Command1 p1 -opt1 10");

            Assert.AreEqual(options.p1, "p1");
            Assert.AreEqual(options.opt1, 10);
        }
   }
}