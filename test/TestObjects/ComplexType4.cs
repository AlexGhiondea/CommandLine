using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests.TestObjects
{
    // define two required collection properties but on different argument groups
    class ComplexType4
    {
        [ActionArgument]
        public CommandAction Action { get; set; }

        [ArgumentGroup(nameof(CommandAction.Check))]
        [RequiredArgument(0, "list1", "", true)]
        public List<string> List1 { get; set; }

        [ArgumentGroup(nameof(CommandAction.List))]
        [RequiredArgument(0, "list2", "", true)]
        public List<string> List2 { get; set; }
    }
}
