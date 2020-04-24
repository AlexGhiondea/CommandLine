using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CommandLine.Analyzer
{
    public partial class CommandLineAnalyzer : DiagnosticAnalyzer
    {
        /*

[DONE]
Error
- Do not allow multiple required properties with the same position
- Do not allow multiple optional properties with the same name
- Do not allow multiple action arguments on the same type
- Do not allow a CommonArgument/AttributeGroup if no required/optional attributes are also present
- Do not allow a CommonArgument if the ActionArgument is not an enum
- Do not allow both required and optional arguments on the same property

Warning
- Do not have the same name for optional and required properties.
- An action argument requires action specific arguments to be specified
- An argumentGroup on an property without a required/optional property does nothing

[REMAINING]
Info
- Specifying an argument as common/argumentgroup without an action argument does nothing
             */

        private const string Category = "CommandLine.NET";

        #region Errors
        private static DiagnosticDescriptor DuplicatePositionalArgumentPositionRule =
            new DiagnosticDescriptor("CMDNET01", "Multiple required properties must not use the same position", "The class defines two required properties on the same position ('{0}').", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        private static DiagnosticDescriptor DuplicateArgumentNameRule =
            new DiagnosticDescriptor("CMDNET02", "Multiple properties must not use the same name", "The class defines two propeties with the same name ('{0}').", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        private static DiagnosticDescriptor CannotSpecifyAGroupForANonPropertyRule =
            new DiagnosticDescriptor("CMDNET03", "A property must be either a required or optional property in order for group assignments to be allowed.", "The CommonArgumentAttribute/ArgumentGroupAttribute can only be applied to properties that already have the [RequiredArgument] or [OptionalArgument] attribute.", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        private static DiagnosticDescriptor CommonArgumentAttributeUsedWhenActionArgumentNotEnumRule =
            new DiagnosticDescriptor("CMDNET04", "The action property must be an Enum in order to use the CommonArgumentAttribute", "The CommonArgumentAttribute can only used when the action argument has a known, finite set of item, ie. an Enum.", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        private static DiagnosticDescriptor DuplicateActionArgumentRule =
            new DiagnosticDescriptor("CMDNET05", "Only one action argument is allowed per type", "The type can only define one action argument.", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        private static DiagnosticDescriptor ConflictingPropertyDeclarationRule =
            new DiagnosticDescriptor("CMDNET06", "Only one argument type is allowed per property", "The property cannot be both required and optional.", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        private static DiagnosticDescriptor RequiredPositionalArgumentNotFound =
            new DiagnosticDescriptor("CMDNET07", "Required argument not found", "The type declares '{0}' properties as required. The property positions are 0-based. Could not find required argument at position '{1}'.", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        private static DiagnosticDescriptor CollectionArgumentShouldBeLast =
            new DiagnosticDescriptor("CMDNET08", "Required collection argument should be the last argument.", "The collection argument '{0}' needs to be the last argument in the list. Otherwise, it will not be possible to parse it at runtime.", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        private static DiagnosticDescriptor OnlyOneRequiredCollection =
            new DiagnosticDescriptor("CMDNET09", "Only one required collection argument is allowed", "Both arguments '{0}' and '{1}' are marked as required collection arguments. Only one can be required. The other should be changed to optional.", Category, DiagnosticSeverity.Error, isEnabledByDefault: true);

        #endregion

        #region Warnings
        private static DiagnosticDescriptor ActionWithoutArgumentsInGroup =
            new DiagnosticDescriptor("CMDNET11", "Action argument specified but no action specific arguments found", "The class defines a property as an action but there are no action specific properties defined.", Category, DiagnosticSeverity.Warning, isEnabledByDefault: true);
        #endregion

        #region Info
        #endregion

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    DuplicatePositionalArgumentPositionRule,
                    DuplicateArgumentNameRule,
                    CannotSpecifyAGroupForANonPropertyRule,
                    CommonArgumentAttributeUsedWhenActionArgumentNotEnumRule,
                    DuplicateActionArgumentRule,
                    ConflictingPropertyDeclarationRule,
                    ActionWithoutArgumentsInGroup,
                    RequiredPositionalArgumentNotFound,
                    CollectionArgumentShouldBeLast,
                    OnlyOneRequiredCollection
                );
            }
        }
    }
}
