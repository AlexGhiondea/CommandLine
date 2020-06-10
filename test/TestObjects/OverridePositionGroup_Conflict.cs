using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class OverridePositionGroup_Conflict
    {
        [ActionArgument]
        public Action Action { get; set; }

        [ArgumentGroup(nameof(Action.List), 0)]
        [RequiredArgument(1, "repos", "The list of repositories where to add the milestones to. The format is: owner\\repoName.", true)]
        public List<string> Repositories { get; set; }

        [ArgumentGroup(nameof(Action.List))]
        [RequiredArgument(0, "test", "The list of repositories where to add the milestones to. The format is: owner\\repoName.", true)]
        public List<string> Repositories2 { get; set; }
    }
}
