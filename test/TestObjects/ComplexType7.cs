using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests.TestObjects
{
    // define required collection property as first one
    class ComplexType7
    {
        [ActionArgument]
        public CommandAction Action { get; set; }

        [CommonArgument]
        [OptionalArgument(null, "token", "The GitHub authentication token.")]
        public string Token { get; set; }

        [RequiredArgument(2, "type", "The type of object we want to list")]
        [ArgumentGroup(nameof(CommandAction.List))]
        public ObjectType ObjectType { get; set; }

        [ArgumentGroup(nameof(CommandAction.Check))]
        [ArgumentGroup(nameof(CommandAction.Create))]
        [ArgumentGroup(nameof(CommandAction.CreateOrUpdate))]
        [RequiredArgument(1, "objectsFile", "The file containing the list of objects to create or check. The structure is: <type>,<name>,<description>,[any other type specific information]")]
        public string ObjectsFile { get; set; }

        [ArgumentGroup(nameof(CommandAction.Check))]
        [ArgumentGroup(nameof(CommandAction.List))]
        [ArgumentGroup(nameof(CommandAction.Create))]
        [ArgumentGroup(nameof(CommandAction.CreateOrUpdate))]
        [RequiredArgument(0, "repos", "The list of repositories where to add the milestones to.", true)]
        public List<string> Repositories { get; set; }
    }
}
