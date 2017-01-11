using CommandLine.Analysis;
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CommandLine
{
    public static class Parser
    {
        public static TOptions Parse<TOptions>(string[] args) where TOptions : new()
        {
            TOptions options = default(TOptions);
            TryParse(args, out options);
            return options;
        }

        public static bool TryParse<TOptions>(string[] args, out TOptions options) where TOptions : new()
        {
            options = default(TOptions);

            TypeArgumentInfo arguments = null;
            try
            {
                // build a list of properties for the type passed in.
                // this will throw for cases where the type is incorrecly annotated with attributes
                TypeHelpers.ScanTypeForProperties<TOptions>(out arguments);

                // short circuit the request for help!
                if (args.Length == 1 && (args[0] == "/?" || args[0] == "-?" || args[0] == "--help"))
                {
                    HelpGenerator.DisplayHelp(args[0], arguments);
                    return false;
                }

                // we have groups!
                if (!arguments.ArgumentGroups.ContainsKey(string.Empty))
                {
                    return ParseCommandGroups(args, ref options, arguments);
                }

                // parse the arguments and build the options object
                options = InternalParse<TOptions>(args, 0, arguments.ArgumentGroups[string.Empty]);
                return true;
            }
            catch (Exception ex)
            {
                Colorizer.WriteLine($"[Red!Error]: {ex.Message} {Environment.NewLine}");

                HelpGenerator.DisplayHelp(HelpGenerator.RequestShortHelpParameter, arguments);

                return false;
            }
        }

        private static bool ParseCommandGroups<TOptions>(string[] args, ref TOptions options, TypeArgumentInfo arguments) where TOptions : new()
        {
            if (arguments.ActionArgument == null)
            {
                throw new ArgumentException("Cannot have groups unless Command argument has been specified");
            }

            // parse based on the command passed in (the first arg).
            if (!arguments.ArgumentGroups.ContainsKey(args[0]))
            {
                throw new ArgumentException($"Unknown command [Cyan!{args[0]}]");
            }

            // short circuit the request for help!
            if (args.Length == 2 && (args[1] == "/?" || args[1] == "-?"))
            {
                HelpGenerator.DisplayHelpForCommmand(args[0], arguments.ArgumentGroups[args[0]]);
                return false;
            }

            options = InternalParse<TOptions>(args, 1, arguments.ArgumentGroups[args[0]]);
            arguments.ActionArgument.SetValue(options, PropertyHelpers.GetValueAsType(args[0], arguments.ActionArgument));
            return true;
        }

        private static TOptions InternalParse<TOptions>(string[] args, int offsetInArray, ArgumentGroupInfo arguments) where TOptions : new()
        {
            TOptions options = new TOptions();
            int currentLogicalPosition = 0;

            // let's match them to actual required args, in positional
            if (arguments.RequiredArguments.Count > 0)
            {
                ParseRequiredParameters(args, offsetInArray, arguments, options, ref currentLogicalPosition);
            }

            // we are going to keep track of any properties that have not been specified so that we can set their default value.
            var unmatchedOptionalProperties = ParseOptionalParameters(args, offsetInArray, arguments, options, ref currentLogicalPosition);

            // for all the remaining optional properties, set their default value.
            foreach (var property in unmatchedOptionalProperties)
            {
                //get the default value..
                var value = property.GetCustomAttribute<OptionalArgumentAttribute>();
                property.SetValue(options, Convert.ChangeType(value.DefaultValue, property.PropertyType));
            }

            return options;
        }

        private static List<PropertyInfo> ParseOptionalParameters<TOptions>(string[] args, int offsetInArray, ArgumentGroupInfo TypeArgumentInfo, TOptions options, ref int currentLogicalPosition) where TOptions : new()
        {
            // we are going to assume that all optionl parameters are not matched to values in 'args'
            List<PropertyInfo> unmatched = new List<PropertyInfo>(TypeArgumentInfo.OptionalArguments.Values);
            // process the optional arguments
            while (offsetInArray + currentLogicalPosition < args.Length)
            {
                if (args[offsetInArray + currentLogicalPosition][0] != '-')
                {
                    throw new ArgumentException("Optional parameter name should start with '-'");
                }

                PropertyInfo optionalProp = null;
                var optionalParamName = args[offsetInArray + currentLogicalPosition].Substring(1);
                if (!TypeArgumentInfo.OptionalArguments.TryGetValue(optionalParamName, out optionalProp))
                {
                    throw new ArgumentException($"Could not find argument {args[currentLogicalPosition]}");
                }

                // skip over the parameter name
                currentLogicalPosition++;

                var value = PropertyHelpers.GetValueFromArgsArray(args, offsetInArray, ref currentLogicalPosition, optionalProp);

                optionalProp.SetValue(options, value);
                unmatched.Remove(optionalProp);
            }

            return unmatched;
        }

        private static void ParseRequiredParameters<TOptions>(string[] args, int offsetInArray, ArgumentGroupInfo TypeArgumentInfo, TOptions options, ref int currentLogicalPosition) where TOptions : new()
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Required parameters have not been specified");
            }

            int matchedRequiredParameters = 0;
            do
            {
                //set the required property
                PropertyInfo propInfo;
                if (!TypeArgumentInfo.RequiredArguments.TryGetValue(currentLogicalPosition, out propInfo))
                {
                    break;
                }

                int paramPosition = currentLogicalPosition; // GetValue changes the current position

                //make sure that we don't run out of array
                if (offsetInArray + currentLogicalPosition >= args.Length)
                {
                    throw new ArgumentException("Required parameters have not been specified");
                }
                var value = PropertyHelpers.GetValueFromArgsArray(args, offsetInArray, ref currentLogicalPosition, propInfo);

                propInfo.SetValue(options, value);
                matchedRequiredParameters++;
            } while (currentLogicalPosition < args.Length);

            // no more? do we have any properties that we have not yet set?
            if (TypeArgumentInfo.RequiredArguments.Count != matchedRequiredParameters)
            {
                throw new ArgumentException("Not all required arguments have been specified");
            }
        }
    }
}