using CommandLine.Attributes;
using System.Collections.Generic;

namespace CommandLine.Tests.TestObjects
{
    // define two optional collection properties
    class ComplexType3
    {
        [OptionalArgument(null, "repos", "The list of repositories where to add the milestones to.", true)]
        public List<string> Repositories { get; set; }

        [OptionalArgument(null, "list", "Another list", true)]
        public List<string> List { get; set; }
    }
}
