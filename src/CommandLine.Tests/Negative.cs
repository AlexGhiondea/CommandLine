using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLine.Tests
{
    [TestClass]
    public class Negative
    {
        [TestMethod]
        public void NegativeTest1()
        {
            var options = Helpers.Parse<OptionsNegative_BadDefaultValue>("-opt2 b");

            Assert.AreEqual(null, options);
        }

        [TestMethod]
        public void NegativeTest2()
        {
            var options = Helpers.Parse<OptionsNegative1>("a");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void NegativeTest3()
        {
            var options = Helpers.Parse<OptionsNegative1>("");
            Assert.AreEqual(options, null);
        }
    }
}