using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using CommandLine;
using CommandLine.Analyzer;

namespace CommandLine.Test
{
    [TestClass]
    public class UnitTest : DiagnosticVerifier
    {
        [TestCategory("Analyzer")]
        [TestMethod]
        public void NoErrorExpectedForNoCode()
        {
            var test = @"";

            VerifyCommandLineDiagnostic(test);
        }

        [TestMethod]
        [TestCategory("Samples")]
        public void NoErrorExpectedForSample_Simple()
        {
            var test = @"
using CommandLine.Attributes;
class Options
{
    [RequiredArgument(0,""dir"",""Directory"")]
    public string Directory { get; set; }

    [OptionalArgument(""*.*"", ""pattern"", ""Search pattern"")]
    public string SearchPattern { get; set; }
}
";

            VerifyCommandLineDiagnostic(test);
        }

        [TestMethod]
        [TestCategory("Samples")]
        public void NoErrorExpectedForSample_Advanced()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

internal enum CommandLineActionGroup { add, remove }
internal class CommandLineOptions
{
    [ActionArgument]
    public CommandLineActionGroup Action { get; set; }

    [CommonArgument]
    [RequiredArgumentAttribute(0, ""host"", ""The name of the host"")]
    public string Host { get; set; }

    #region Add-host Action
    [RequiredArgument(1, ""mac"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC { get; set; }

    #endregion
}
    ";

            VerifyCommandLineDiagnostic(test);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void TwoPropertiesOnTheSamePosition()
        {
            var test = @"
using CommandLine.Attributes;
class Options
{
    [RequiredArgument(0, ""dir"", ""Directory"")]
    public string Directory { get; set; }

    [RequiredArgument(0, ""bar"", ""Directory2"")]
    public string Directory2 { get; set; }
}";

            var expected1 = new DiagnosticResult
            {
                Id = "CMDNET01",
                Message = "The class defines two required properties on the same position ('0').",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 19)
                        }
            };

            var expected2 = new DiagnosticResult
            {
                Id = "CMDNET07",
                Message = "The type declares '2' properties as required. The property positions are 0-based. Could not find required argument at position '1'.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 3, 7)
                        }
            };

            VerifyCommandLineDiagnostic(test, expected2, expected1);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void TwoPropertiesWithTheSameName_BothRequired()
        {
            var test = @"
using CommandLine.Attributes;
class Options
{
    [RequiredArgument(0, ""dir"", ""Directory"")]
    public string Directory { get; set; }

    [RequiredArgument(1, ""dir"", ""Directory2"")]
    public string Directory2 { get; set; }
}";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET02",
                Message = "The class defines two propeties with the same name ('dir').",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 19)
                        }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void TwoPropertiesWithTheSameName_BothOptional()
        {
            var test = @"
using CommandLine.Attributes;
class Options
{
    [OptionalArgument(""0"", ""dir"", ""Directory"")]
    public string Directory { get; set; }

    [OptionalArgument(""1"", ""dir"", ""Directory2"")]
    public string Directory2 { get; set; }
}";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET02",
                Message = "The class defines two propeties with the same name ('dir').",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 19)
                        }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void TwoPropertiesWithTheSameName_RequiredAndOptional()
        {
            var test = @"
using CommandLine.Attributes;
class Options
{
    [RequiredArgument(0, ""dir"", ""Directory"")]
    public string Directory { get; set; }

    [OptionalArgument(""1"", ""dir"", ""Directory2"")]
    public string Directory2 { get; set; }
}";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET02",
                Message = "The class defines two propeties with the same name ('dir').",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 19)
                        }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void RequiredAndOptionalArgumentOnProperty()
        {
            var test = @"
using CommandLine.Attributes;
class Options
{
    [OptionalArgument(""aa"", ""dir"", ""Directory"")]
    [RequiredArgument(0, ""dir"", ""Directory"")]
    public string Directory { get; set; }
}";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET06",
                Message = "The property cannot be both required and optional.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 7, 19)
                        }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void MultipleActionArgumentsOnType()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
class Options
{
    [ActionArgument]    
    public string Action1 { get; set; }

    [ActionArgument]
    public string Action2 { get; set; }

    [ArgumentGroup(""test"")]
    [OptionalArgument(""1"", ""dir"", ""Directory2"")]
    public string Directory { get; set; }

}";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET05",
                Message = "The type can only define one action argument.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 10, 19)
                        }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void ArgumentGroupSpecifiedOnNonProperty()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
class Options
{
    [ActionArgument]    
    public string Action1 { get; set; }

    //ERROR here!
    [ArgumentGroup(""test"")]
    public string Directory { get; set; }

    [ArgumentGroup(""test"")]
    [OptionalArgument(""1"", ""dir"", ""Directory2"")]
    public string Directory2 { get; set; }
}";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET03",
                Message = "The CommonArgumentAttribute/ArgumentGroupAttribute can only be applied to properties that already have the [RequiredArgument] or [OptionalArgument] attribute.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 19)
                        }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void CommonArgumentSpecifiedOnStringActionField()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
class Options
{
    [ActionArgument]    
    public string Action1 { get; set; }

    [CommonArgument]
    [OptionalArgument(""1"", ""dir"", ""Directory2"")]
    public string Directory2 { get; set; }
}";

            var expected = new[] {
                new DiagnosticResult
                {
                    Id = "CMDNET11",
                    Message = "The class defines a property as an action but there are no action specific properties defined.",
                    Severity = DiagnosticSeverity.Warning,
                    Locations =
                        new[] {
                                new DiagnosticResultLocation("Test0.cs", 7, 19)
                            }
                },
                new DiagnosticResult
                {
                    Id = "CMDNET04",
                    Message = "The CommonArgumentAttribute can only used when the action argument has a known, finite set of item, ie. an Enum.",
                    Severity = DiagnosticSeverity.Error,
                    Locations =
                        new[] {
                                new DiagnosticResultLocation("Test0.cs", 11, 19)
                            }
                }
            }
            ;

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void RequiredPropertyNotStartingAtPositionZero()
        {
            var test = @"
using CommandLine.Attributes;
class Options
{
    [RequiredArgument(1, ""dir"", ""Directory"")]
    public string Directory { get; set; }
}";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET07",
                Message = "The type declares '1' properties as required. The property positions are 0-based. Could not find required argument at position '0'.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 3, 7)
                        }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void PropertiesWithMultipledPositionalRequiredArgsInGroups()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

internal enum CommandLineActionGroup { add, remove }
internal class CommandLineOptions
{
    [ActionArgument]
    public CommandLineActionGroup Action { get; set; }

    [CommonArgument]
    [RequiredArgumentAttribute(0, ""host"", ""The name of the host"")]
    public string Host { get; set; }

    #region Add-host Action
    [RequiredArgument(1, ""mac1"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC { get; set; }

    [RequiredArgument(2, ""mac2"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC2 { get; set; }

    [RequiredArgument(3, ""mac3"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC3 { get; set; }
    #endregion

    #region remove-host Action
    [RequiredArgument(1, ""mac1"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.remove))]
    public string MAC_r { get; set; }

    [RequiredArgument(2, ""mac2"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.remove))]
    public string MAC2_r { get; set; }

    [RequiredArgument(3, ""mac3"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.remove))]
    public string MAC3_r { get; set; }
    #endregion
}
    ";

            VerifyCommandLineDiagnostic(test);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void PropertiesWithMultipledPositionalRequiredArgsInGroups2()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

internal enum CommandLineActionGroup { add, remove }
internal class CommandLineOptions
{
    [ActionArgument]
    public CommandLineActionGroup Action { get; set; }

    [CommonArgument]
    [RequiredArgumentAttribute(0, ""host"", ""The name of the host"")]
    public string Host { get; set; }

    [CommonArgument]
    [RequiredArgumentAttribute(2, ""host2"", ""The name of the host"")]
    public string Host2 { get; set; }

    #region Add-host Action
    [RequiredArgument(1, ""mac1"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC { get; set; }
    #endregion

    #region remove-host Action
    [RequiredArgument(1, ""mac1"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.remove))]
    public string MAC_r { get; set; }
    #endregion
}
    ";

            VerifyCommandLineDiagnostic(test);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void InvalidPropertiesWithMultipledPositionalRequiredArgsInGroups()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

internal enum CommandLineActionGroup { add, remove }
internal class CommandLineOptions
{
    [ActionArgument]
    public CommandLineActionGroup Action { get; set; }

    [CommonArgument]
    [RequiredArgumentAttribute(0, ""host"", ""The name of the host"")]
    public string Host { get; set; }

    #region Add-host Action
    [RequiredArgument(1, ""mac1"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC { get; set; }

    [RequiredArgument(2, ""mac2"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC2 { get; set; }

    [RequiredArgument(3, ""mac3"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC3 { get; set; }
    #endregion

    #region remove-host Action
    [RequiredArgument(1, ""mac1"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.remove))]
    public string MAC_r { get; set; }

    [RequiredArgument(3, ""mac2"", ""The MAC address of the host"")]
    [ArgumentGroup(nameof(CommandLineActionGroup.remove))]
    public string MAC2_r { get; set; }
    #endregion
}
    ";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET07",
                Message = "The type declares '3' properties as required. The property positions are 0-based. Could not find required argument at position '2'.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                               new[] {
                            new DiagnosticResultLocation("Test0.cs", 6, 16)
                                   }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void RequiredCollectionArgShouldBeTheOnlyOne()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

    internal class CmdLineArgs
    {
        [ActionArgument]
        public Action Action { get; set; }

        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(0, ""milestoneInputFile"", ""The file containing the list of milestones to create."")]
        public string MilestoneFile { get; set; }

        [ArgumentGroup(nameof(Action.List), overrideRequiredPosition: 0)]
        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(1, ""repos"", ""The list of repositories where to add the milestones to. The format is: owner\\repoName."", true)]
        public List<string> Repositories { get; set; }

        [ArgumentGroup(nameof(Action.List))]
        [RequiredArgument(1, ""repos2"", ""The list of repositories where to add the milestones to. The format is: owner\\repoName."", true)]
        public List<string> Repositories2 { get; set; }
    }

    public enum Action
    {
        Create,
        List
    }

";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET09",
                Message = "Both arguments 'repos' and 'repos2' are marked as required collection arguments. Only one can be required. The other should be changed to optional.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                 new[] {
                            new DiagnosticResultLocation("Test0.cs", 21, 29)
                     }
            };

            VerifyCommandLineDiagnostic(test,expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void RequiredArgumentShouldBeLast()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

    internal class CmdLineArgs
    {
        [RequiredArgument(1, ""milestoneInputFile"", ""The file containing the list of milestones to create."")]
        public string MilestoneFile { get; set; }

        [RequiredArgument(0, ""repos"", ""The list of repositories where to add the milestones to. The format is: owner\\repoName."", true)]
        public List<string> Repositories { get; set; }
    }

    public enum Action
    {
        Create,
        List
    }

";
            var expected = new DiagnosticResult
            {
                Id = "CMDNET08",
                Message = "The collection argument 'repos' needs to be the last argument in the list. Otherwise, it will not be possible to parse it at runtime.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                 new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 29)
                     }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void InvalidCode1()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
class Options
{
    [Action    
    public string Action1 { get; set; }

    [ActionArgument]
    public string Action2 { get; set; }

    [ArgumentGroup(""test"")]
    [OptionalArgument(""1"", ""dir"", ""Directory2"")]
    public string Directory { get; set; }

}";

            VerifyCommandLineDiagnostic(test);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void InvalidCode2()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
class Options
{
    []
    public string Action1 { get; set; }

    [ActionArgument]
    public string Action2 { get; set; }

    [ArgumentGroup(""test"")]
    [OptionalArgument(""1"", ""dir"", ""Directory2"")]
    public string Directory { get; set; }

}";

            VerifyCommandLineDiagnostic(test);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void InvalidCode3()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
class Options
{
    []
    public string Action1

    [ActionArgument]
    public string Action2 { get; set; }

    [ArgumentGroup(""test"")]
    [OptionalArgument(""1"", ""dir"", ""Directory2"")]
    public string Directory { get; set; }

}";

            VerifyCommandLineDiagnostic(test);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void InvalidCode4()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
class Options
{
    [OptionalArgument(""1"", ""dir"")]
    public string Directory { get; set; }

}";

            VerifyCommandLineDiagnostic(test);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void ArgumentWithNoArgs()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

namespace DotNetVersions.Console
{
    class Options
    {
        [ArgumentGroup(nameof(Action.search))]
        [RequiredArgument(0, ""version"", ""The file version you are interested in"")]
        public string Version { get; set; }

        [ActionArgument]
        public Action Action { get; set; }
    }

    enum Action
    {
        list,
        search
    }
}

";
            VerifyCommandLineDiagnostic(test);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void TwoRequiredCollectionProperties()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLine.Tests.TestObjects
{
    class ComplexType2
    {
        [RequiredArgument(0, ""repos"", ""The list of repositories where to add the milestones to."", true)]
        public List<string> Repositories { get; set; }

        [RequiredArgument(1, ""list"", ""Another list"", true)]
        public List<string> List { get; set; }
    }
}";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET09",
                Message = "Both arguments 'repos' and 'list' are marked as required collection arguments. Only one can be required. The other should be changed to optional.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                   new[] {
                            new DiagnosticResultLocation("Test0.cs", 16, 29)
                       }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        [TestCategory("Analyzer")]
        [TestMethod]
        public void TwoCollectionPropertiesOnArgumentGroup()
        {
            var test = @"
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;

    internal class CmdLineArgs
    {
        [ActionArgument]
        public Action Action { get; set; }

        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(0, ""milestoneInputFile"", ""The file containing the list of milestones to create."")]
        public string MilestoneFile { get; set; }

        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(1, ""repos"", ""The list of repositories where to add the milestones to. The format is: owner\\repoName."", true)]
        public List<string> Repositories { get; set; }

        [ArgumentGroup(nameof(Action.Create))]
        [RequiredArgument(2, ""repos2"", ""The list of repositories where to add the milestones to. The format is: owner\\repoName."", true)]
        public List<string> Repositories2 { get; set; }
    }

    public enum Action
    {
        Create,
        List
    }

";

            var expected = new DiagnosticResult
            {
                Id = "CMDNET09",
                Message = "Both arguments 'repos' and 'repos2' are marked as required collection arguments. Only one can be required. The other should be changed to optional.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                 new[] {
                            new DiagnosticResultLocation("Test0.cs", 20, 29)
                     }
            };

            VerifyCommandLineDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CommandLineAnalyzer();
        }
    }
}
