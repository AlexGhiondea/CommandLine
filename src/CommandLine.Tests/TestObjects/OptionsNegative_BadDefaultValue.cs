using CommandLine.Attributes;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class OptionsNegative_BadDefaultValue
    {
        [OptionalArgument("opt2", "a", "Optional parameter 2", true)]
        public List<string> opt2 { get; set; }
    }
}
