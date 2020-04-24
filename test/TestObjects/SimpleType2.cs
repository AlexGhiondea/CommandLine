using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests.TestObjects
{
    // define required collection property as first one
    class SimpleType2
    {
        [RequiredArgument(0, "list1", "", true)]
        public SimpleType2 List1 { get; set; }
    }
}
