using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLine.Tests
{
    class OverridePositionGroup
    {
        [ActionArgument]
        public Action Action { get; set; }

        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(0, "milestoneInputFile", "The file containing the list of milestones to create.")]
        public string MilestoneFile { get; set; }

        [ArgumentGroup(nameof(Action.List), 0)]
        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(1, "repos", "The list of repositories where to add the milestones to. The format is: owner\\repoName.", true)]
        public List<string> Repositories { get; set; }
    }

    class OverridePositionGroup_Conflict
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

    public enum Action
    {
        Create,
        List
    }
}
