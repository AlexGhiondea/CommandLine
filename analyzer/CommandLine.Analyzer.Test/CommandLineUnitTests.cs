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
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CommandLineAnalyzer();
        }
    }
}
