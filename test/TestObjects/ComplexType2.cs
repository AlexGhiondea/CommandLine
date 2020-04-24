using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Tests.TestObjects
{
    class ComplexType2
    {
        [RequiredArgument(0, "repos", "The list of repositories where to add the milestones to.", true)]
        public List<string> Repositories { get; set; }

        [RequiredArgument(1, "list", "Another list", true)]
        public List<string> List { get; set; }
    }
}
