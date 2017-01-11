using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLine.Tests
{
    [TestClass]
    public class Functional
    {
        [TestMethod]
        public void BasicTest1()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt1 10 -opt2 b");

            Assert.AreEqual(options.p1, "p1");
            Assert.AreEqual(options.p2, 2);
            Assert.AreEqual(options.opt1, 10);
            Assert.AreEqual(options.opt2, "b");
            Assert.AreEqual(options.opt3, null);
        }

        [TestMethod]
        public void BasicTest2()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt1 10");

            Assert.AreEqual(options.p1, "p1");
            Assert.AreEqual(options.p2, 2);
            Assert.AreEqual(options.opt1, 10);
            Assert.AreEqual(options.opt2, "all");
            Assert.AreEqual(options.opt3, null);
        }

        [TestMethod]
        public void BasicTest3()
        {
            var options = Helpers.Parse<Options1>("p1 2");

            Assert.AreEqual(options.p1, "p1");
            Assert.AreEqual(options.p2, 2);
            Assert.AreEqual(options.opt1, 256);
            Assert.AreEqual(options.opt2, "all");
            Assert.AreEqual(options.opt3, null);
        }

        [TestMethod]
        public void BasicTest4()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt3 a b c");

            Assert.AreEqual(options.p1, "p1");
            Assert.AreEqual(options.p2, 2);
            Assert.AreEqual(options.opt1, 256);
            Assert.AreEqual(options.opt2, "all");
            Helpers.CollectionEquals(options.opt3, "a", "b", "c");
        }

        [TestMethod]
        public void BasicTest5()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt1 10 -opt3 a b c");

            Assert.AreEqual(options.p1, "p1");
            Assert.AreEqual(options.p2, 2);
            Assert.AreEqual(options.opt1, 10);
            Assert.AreEqual(options.opt2, "all");
            Helpers.CollectionEquals(options.opt3, "a", "b", "c");
        }

        [TestMethod]
        public void BasicTest6()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt3 a b c -opt1 10");

            Assert.AreEqual(options.p1, "p1");
            Assert.AreEqual(options.p2, 2);
            Assert.AreEqual(options.opt1, 10);
            Assert.AreEqual(options.opt2, "all");
            Helpers.CollectionEquals(options.opt3, "a", "b", "c");
        }

        [TestMethod]
        public void BasicTest7()
        {
            var options = Helpers.Parse<Options2>("p1 d e fc -opt2 a b c -opt1 10");

            Assert.AreEqual(options.p1, "p1");
            Helpers.CollectionEquals(options.p2, "d", "e", "fc");
            Assert.AreEqual(options.opt1, 10);
            Helpers.CollectionEquals(options.opt2, "a", "b", "c");
        }

        [TestMethod]
        public void BasicTest8()
        {
            var options = Helpers.Parse<Options2>("p1 d e fc");

            Assert.AreEqual(options.p1, "p1");
            Helpers.CollectionEquals(options.p2, "d", "e", "fc");
            Assert.AreEqual(options.opt1, 256);
            Assert.AreEqual(options.opt2, null);
        }

        [TestMethod]
        public void BasicTest9()
        {
            var options = Helpers.Parse<Options3NoRequired>("-opt1 10 -opt2 d e fc");

            Assert.AreEqual(options.opt1, 10);
            Helpers.CollectionEquals(options.opt2, "d", "e", "fc");
            Assert.AreEqual(options.opt3, Enum1.B);
        }

        [TestMethod]
        public void BasicTest10()
        {
            var options = Helpers.Parse<Options3NoRequired>("-opt1 10 -opt2 d e fc -opt3 A");

            Assert.AreEqual(options.opt1, 10);
            Helpers.CollectionEquals(options.opt2, "d", "e", "fc");
            Assert.AreEqual(options.opt3, Enum1.A);
        }

    }
}