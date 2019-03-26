using CommandLine.Attributes;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class OptionsNegative1
    {
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public Enum1 p1 { get; set; }

        [RequiredArgument(1, "p2", "Required parameter 2", true)]
        public List<string> p2 { get; set; }
    }

    internal class OptionsNegative2
    {
        [OptionalArgument("foo", "aaa", "")]
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public Enum1 p1 { get; set; }
    }

    internal class OptionsNegative2_multipleReqSamePos
    {
        [RequiredArgument(0, "p1", "Required parameter 1")]
        public Enum1 p1 { get; set; }

        [RequiredArgument(0, "p2", "Required parameter 2")]
        public Enum1 p2 { get; set; }
    }

    internal class OptionsNegative2_multipleOptSameName
    {
        [OptionalArgument(0, "p1", "Required parameter 1")]
        public Enum1 p1 { get; set; }

        [OptionalArgument(0, "p1", "Required parameter 2")]
        public Enum1 p2 { get; set; }
    }

    internal class OptionsNegative2_CommandMultiple
    {
        [Attributes.Advanced.ActionArgument]
        public Enum1 p1 { get; set; }

        [Attributes.Advanced.ActionArgument]
        public Enum1 p2 { get; set; }
    }

    internal class RequiredNegative_InvalidPositionForRequiredArg
    {
        [RequiredArgument(1, "a", "")]
        public Enum1 p1 { get; set; }
    }

    internal class MorePassedInThanRequired
    {
        [RequiredArgument(0, "a", "")]
        public string p1 { get; set; }

        [RequiredArgument(1, "b", "")]
        public string p2 { get; set; }
    }
}
