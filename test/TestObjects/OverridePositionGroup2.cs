using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

namespace CommandLine.Tests
{
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
}
