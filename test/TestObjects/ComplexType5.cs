using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests.TestObjects
{
    // define required collection property as first one
    class ComplexType5
    {
        [RequiredArgument(1, "nonList", "")]
        public string NonList { get; set; }

        [RequiredArgument(0, "list1", "", true)]
        public List<string> List1 { get; set; }
    }
}
