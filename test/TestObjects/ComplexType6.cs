using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests.TestObjects
{
    // define required collection property as first one
    class ComplexType6
    {
        [ActionArgument]
        public Action Action { get; set; }

        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(0, "milestoneInputFile", "The file containing the list of milestones to create.")]
        public string MilestoneFile { get; set; }

        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(1, "repos", "The list of repositories where to add the milestones to.The format is: owner\\repoName.", true)]
        public List<string> Repositories { get; set; }

        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(2, "repos2", "The list of repositories where to add the milestones to.The format is: owner\\repoName.", true)]
        public List<string> Repositories2 { get; set; }
    }
}
