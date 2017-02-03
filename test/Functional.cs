using System;
using Xunit;

namespace CommandLine.Tests
{
    public class Functional
    {
        [Fact]
        public void BasicTest1()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt1 10 -opt2 b");

            Assert.Equal(options.p1, "p1");
            Assert.Equal(options.p2, 2);
            Assert.Equal(options.opt1, 10);
            Assert.Equal(options.opt2, "b");
            Assert.Equal(options.opt3, null);
        }

        [Fact]
        public void BasicTest2()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt1 10");

            Assert.Equal(options.p1, "p1");
            Assert.Equal(options.p2, 2);
            Assert.Equal(options.opt1, 10);
            Assert.Equal(options.opt2, "all");
            Assert.Equal(options.opt3, null);
        }

        [Fact]
        public void BasicTest3()
        {
            var options = Helpers.Parse<Options1>("p1 2");

            Assert.Equal(options.p1, "p1");
            Assert.Equal(options.p2, 2);
            Assert.Equal(options.opt1, 256);
            Assert.Equal(options.opt2, "all");
            Assert.Equal(options.opt3, null);
        }

        [Fact]
        public void BasicTest4()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt3 a b c");

            Assert.Equal(options.p1, "p1");
            Assert.Equal(options.p2, 2);
            Assert.Equal(options.opt1, 256);
            Assert.Equal(options.opt2, "all");
            Helpers.CollectionEquals(options.opt3, "a", "b", "c");
        }

        [Fact]
        public void BasicTest5()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt1 10 -opt3 a b c");

            Assert.Equal(options.p1, "p1");
            Assert.Equal(options.p2, 2);
            Assert.Equal(options.opt1, 10);
            Assert.Equal(options.opt2, "all");
            Helpers.CollectionEquals(options.opt3, "a", "b", "c");
        }

        [Fact]
        public void BasicTest6()
        {
            var options = Helpers.Parse<Options1>("p1 2 -opt3 a b c -opt1 10");

            Assert.Equal(options.p1, "p1");
            Assert.Equal(options.p2, 2);
            Assert.Equal(options.opt1, 10);
            Assert.Equal(options.opt2, "all");
            Helpers.CollectionEquals(options.opt3, "a", "b", "c");
        }

        [Fact]
        public void BasicTest7()
        {
            var options = Helpers.Parse<Options2>("p1 d e fc -opt2 a b c -opt1 10");

            Assert.Equal(options.p1, "p1");
            Helpers.CollectionEquals(options.p2, "d", "e", "fc");
            Assert.Equal(options.opt1, 10);
            Helpers.CollectionEquals(options.opt2, "a", "b", "c");
        }

        [Fact]
        public void BasicTest8()
        {
            var options = Helpers.Parse<Options2>("p1 d e fc");

            Assert.Equal(options.p1, "p1");
            Helpers.CollectionEquals(options.p2, "d", "e", "fc");
            Assert.Equal(options.opt1, 256);
            Assert.Equal(options.opt2, null);
        }

        [Fact]
        public void BasicTest9()
        {
            var options = Helpers.Parse<Options3NoRequired>("-opt1 10 -opt2 d e fc");

            Assert.Equal(options.opt1, 10);
            Helpers.CollectionEquals(options.opt2, "d", "e", "fc");
            Assert.Equal(options.opt3, Enum1.B);
        }

        [Fact]
        public void BasicTest10()
        {
            var options = Helpers.Parse<Options3NoRequired>("-opt1 10 -opt2 d e fc -opt3 A");

            Assert.Equal(options.opt1, 10);
            Helpers.CollectionEquals(options.opt2, "d", "e", "fc");
            Assert.Equal(options.opt3, Enum1.A);
        }

    }
}