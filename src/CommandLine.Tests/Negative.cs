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

        [TestMethod]
        public void NegativeTest4()
        {
            var options = Helpers.Parse<OptionsNegative2>("");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void NegativeTest5()
        {
            var options = Helpers.Parse<OptionsNegative2_multipleReqSamePos>("");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void NegativeTest6()
        {
            var options = Helpers.Parse<OptionsNegative2_multipleOptSameName>("");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void NegativeTest7()
        {
            var options = Helpers.Parse<OptionsNegative2_CommandMultiple>("");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void ExtraParameters1()
        {
            var options = Helpers.Parse<Options2>("a b -opt1 a -opt2 b -opt3 c -opt4 t");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void OptionalParameterDoesNotStartWithMinus()
        {
            var options = Helpers.Parse<Options1>("a 1 opt1 a");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void TypeHasGroupsButDoesNotDefineCommand()
        {
            var options = Helpers.Parse<Groups_NoCommand>("a b opt1 a");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void NotEnoughRequiredParametersSpecified()
        {
            var options = Helpers.Parse<Options1>("a ");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void NotEnoughRequiredParametersSpecifiedForACommand()
        {
            var options = Helpers.Parse<Groups1>("Command1");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void UnknownOptionalArgument()
        {
            var options = Helpers.Parse<Options1>("a 1 -doesNotExist 43");
            Assert.AreEqual(options, null);
        }

        [TestMethod]
        public void ParseUnknownCommand()
        {
            var options = Helpers.Parse<Groups1>("a");
            Assert.AreEqual(options, null);
        }

        public void ParseTypeWithWrongRequiredParamPosition()
        {
            var options = Helpers.Parse<RequiredNegative_InvalidPositionForRequiredArg>("");
            Assert.AreEqual(options, null);
        }
    }
}