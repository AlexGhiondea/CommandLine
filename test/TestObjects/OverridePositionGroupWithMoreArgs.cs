using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

namespace CommandLine.Tests
{
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
}
