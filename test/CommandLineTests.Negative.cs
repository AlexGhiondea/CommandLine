using Xunit;

namespace CommandLine.Tests
{
    public partial class CommandLineTests
    {
        [Fact]
        public void NegativeTest1()
        {
            var options = Helpers.Parse<OptionsNegative_BadDefaultValue>("-opt2 b");

            Assert.Null(options);
        }

        [Fact]
        public void NegativeTest2()
        {
            var options = Helpers.Parse<OptionsNegative1>("a");
            Assert.Null(options);
        }

        [Fact]
        public void NegativeTest3()
        {
            var options = Helpers.Parse<OptionsNegative1>("");
            Assert.Null(options);
        }

        [Fact]
        public void NegativeTest4()
        {
            var options = Helpers.Parse<OptionsNegative2>("");
            Assert.Null(options);
        }

        [Fact]
        public void NegativeTest5()
        {
            var options = Helpers.Parse<OptionsNegative2_multipleReqSamePos>("");
            Assert.Null(options);
        }

        [Fact]
        public void NegativeTest6()
        {
            var options = Helpers.Parse<OptionsNegative2_multipleOptSameName>("");
            Assert.Null(options);
        }

        [Fact]
        public void NegativeTest7()
        {
            var options = Helpers.Parse<OptionsNegative2_CommandMultiple>("");
            Assert.Null(options);
        }

        [Fact]
        public void ExtraParameters1()
        {
            var options = Helpers.Parse<Options2>("a b -opt1 a -opt2 b -opt3 c -opt4 t");
            Assert.Null(options);
        }

        [Fact]
        public void OptionalParameterDoesNotStartWithMinus()
        {
            var options = Helpers.Parse<Options1>("a 1 opt1 a");
            Assert.Null(options);
        }

        [Fact]
        public void TypeHasGroupsButDoesNotDefineCommand()
        {
            var options = Helpers.Parse<Groups_NoCommand>("a b opt1 a");
            Assert.Null(options);
        }

        [Fact]
        public void NotEnoughRequiredParametersSpecified()
        {
            var options = Helpers.Parse<Options1>("a ");
            Assert.Null(options);
        }

        [Fact]
        public void NotEnoughRequiredParametersSpecifiedForACommand()
        {
            var options = Helpers.Parse<Groups1>("Command1");
            Assert.Null(options);
        }

        [Fact]
        public void UnknownOptionalArgument()
        {
            var options = Helpers.Parse<Options1>("a 1 -doesNotExist 43");
            Assert.Null(options);
        }

        [Fact]
        public void ParseUnknownCommand()
        {
            var options = Helpers.Parse<Groups1>("a");
            Assert.Null(options);
        }

        [Fact]
        public void ParseTypeWithWrongRequiredParamPosition()
        {
            var options = Helpers.Parse<RequiredNegative_InvalidPositionForRequiredArg>("");
            Assert.Null(options);
        }

        [Fact]
        public void HelpForInvalidType()
        {
            var options = Helpers.Parse<object>("");
            Assert.Null(options);
        }

        [Fact]
        public void MismatchedQuotes()
        {
            var obj = Helpers.Parse<object>("@\" foo ");
            Assert.Null(obj);
        }
    }
}