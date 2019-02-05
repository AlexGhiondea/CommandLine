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

    class OverridePositionGroupWithMoreArgs
    {
        [ActionArgument]
        public Action Action { get; set; }

        [ArgumentGroup(nameof(Action.List), 1)]
        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(0, "milestoneInputFile", "The file containing the list of milestones to create.")]
        public string One { get; set; }

        [ArgumentGroup(nameof(Action.List), 0)]
        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(1, "milestoneInputFile2", "The file containing the list of milestones to create.")]
        public string Two { get; set; }

        [ArgumentGroup(nameof(Action.List), 2)]
        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(2, "repos", "The list of repositories where to add the milestones to. The format is: owner\\repoName.")]
        public string Three { get; set; }
    }

    class OverridePositionGroup2
    {
        [ActionArgument]
        public Action Action { get; set; }

        [ArgumentGroup(nameof(Action.List), 0)]
        [ArgumentGroup(nameof(Action.Create), 1)]
        [RequiredArgument(0, "milestoneInputFile", "The file containing the list of milestones to create.")]
        public string MilestoneFile { get; set; }

        [ArgumentGroup(nameof(Action.Create), 0)]
        [ArgumentGroup(nameof(Action.List), 1)]
        [RequiredArgument(1, "repo", "The list of repositories where to add the milestones to. The format is: owner\\repoName.")]
        public string Repository { get; set; }
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
