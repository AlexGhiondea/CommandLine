using CommandLine.Attributes;
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

            //TODO: When parsing, we are actually removing the properties from the list of properties on type.
            //      this causes the help generator to actually break down when an exception happens.

            TypePropertyInfo parameters = null;
            try
            {
                // build a list of properties for the type passed in.
                // this will throw for cases where the type is incorrecly annotated with attributes
                ScanTypeForProperties<TOptions>(out parameters);

                // short circuit the request for help!
                if (args.Length == 1 && (args[0] == "/?" || args[0] == "-?" || args[0] == "--help"))
                {
                    HelpGenerator.DisplayHelp(args[0], parameters);
                    return false;
                }

                // we have groups!
                if (!parameters.TypeInfo.ContainsKey(string.Empty))
                {
                    return ParseCommandGroups(args, ref options, parameters);
                }

                // we don't have groups
                if (parameters.ActionCommandProperty != null)
                {
                    throw new ArgumentException("Cannot have Command argument unless groups have been specified");
                }

                // parse the arguments and build the options object
                options = InternalParse<TOptions>(args, 0, parameters.TypeInfo[string.Empty]);
                return true;
            }
            catch (Exception ex)
            {
                Colorizer.WriteLine($"[Red!Error]: {ex.Message} {Environment.NewLine}");

                HelpGenerator.DisplayHelp(HelpGenerator.RequestShortHelpParameter, parameters);

                return false;
            }
        }

        private static bool ParseCommandGroups<TOptions>(string[] args, ref TOptions options, TypePropertyInfo parameters) where TOptions : new()
        {
            if (parameters.ActionCommandProperty == null)
            {
                throw new ArgumentException("Cannot have groups unless Command argument has been specified");
            }

            // parse based on the command passed in (the first arg).
            if (!parameters.TypeInfo.ContainsKey(args[0]))
            {
                throw new ArgumentException($"Unknown command [Cyan!{args[0]}]");
            }

            // short circuit the request for help!
            if (args.Length == 2 && (args[1] == "/?" || args[1] == "-?"))
            {
                HelpGenerator.DisplayHelpForCommmand(args[0], parameters.TypeInfo[args[0]]);
                return false;
            }

            options = InternalParse<TOptions>(args, 1, parameters.TypeInfo[args[0]]);
            parameters.ActionCommandProperty.SetValue(options, Convert.ChangeType(GetValueForProperty(args[0], parameters.ActionCommandProperty), parameters.ActionCommandProperty.PropertyType));
            return true;
        }

        public static object GetValueForProperty(string value, PropertyInfo targetType)
        {
            if (targetType.PropertyType.IsEnum)
            {
                return Enum.Parse(targetType.PropertyType, value);
            }

            return value;
        }

        public static object GetValueFromArgsArray(string[] args, int offsetInArray, ref int currentLogicalPosition, PropertyInfo targetType)
        {
            ActualArgumentAttribute value = targetType.GetCustomAttribute<ActualArgumentAttribute>();
            object argValue = null;

            if (!value.IsCollection)
            {
                argValue = GetValueForProperty(args[offsetInArray + currentLogicalPosition], targetType);
                currentLogicalPosition++;
            }
            else
            {
                // we are going to support just string lists.
                int indexLastEntry = args.Length;
                for (int i = offsetInArray + currentLogicalPosition; i < args.Length; i++)
                {
                    if (args[i][0] == '-')
                    {
                        // stop at the first additional argument
                        indexLastEntry = i;
                        break;
                    }
                }

                // create the list
                string[] list = new string[indexLastEntry - currentLogicalPosition - offsetInArray];
                Array.Copy(args, offsetInArray + currentLogicalPosition, list, 0, indexLastEntry - currentLogicalPosition - offsetInArray);

                argValue = new List<string>(list);
                currentLogicalPosition = indexLastEntry;
            }

            return Convert.ChangeType(argValue, targetType.PropertyType);
        }

        private static TOptions InternalParse<TOptions>(string[] args, int offsetInArray, GroupPropertyInfo parameters) where TOptions : new()
        {
            TOptions options = new TOptions();
            int currentLogicalPosition = 0;

            // let's match them to actual required args, in positional
            if (parameters.requiredParam.Count > 0)
            {
                ParseRequiredParameters(args, offsetInArray, parameters, options, ref currentLogicalPosition);
            }

            // we are going to keep track of any properties that have not been specified so that we can set their default value.
            var unmatchedOptionalProperties = ParseOptionalParameters(args, offsetInArray, parameters, options, ref currentLogicalPosition);

            if (currentLogicalPosition + offsetInArray < args.Length)
            {
                //unknown parameters
                throw new ArgumentException("Unknown extra parameters");
            }

            // for all the remaining optional properties, set their default value.
            foreach (var property in unmatchedOptionalProperties)
            {
                //get the default value..
                var value = property.GetCustomAttribute<OptionalArgumentAttribute>();
                property.SetValue(options, Convert.ChangeType(value.DefaultValue, property.PropertyType));
            }

            return options;
        }

        private static List<PropertyInfo> ParseOptionalParameters<TOptions>(string[] args, int offsetInArray, GroupPropertyInfo parameters, TOptions options, ref int currentLogicalPosition) where TOptions : new()
        {
            // we are going to assume that all optionl parameters are not matched to values in 'args'
            List<PropertyInfo> unmatched = new List<PropertyInfo>(parameters.optionalParam.Values);
            // process the optional arguments
            while (offsetInArray + currentLogicalPosition < args.Length)
            {
                if (args[offsetInArray + currentLogicalPosition][0] != '-')
                {
                    throw new ArgumentException("Optional parameter name should start with '-'");
                }

                PropertyInfo optionalProp = null;
                var optionalParamName = args[offsetInArray + currentLogicalPosition].Substring(1);
                if (!parameters.optionalParam.TryGetValue(optionalParamName, out optionalProp))
                {
                    throw new ArgumentException($"Could not find argument {args[currentLogicalPosition]}");
                }

                // skip over the parameter name
                currentLogicalPosition++;

                var value = GetValueFromArgsArray(args, offsetInArray, ref currentLogicalPosition, optionalProp);

                optionalProp.SetValue(options, value);
                unmatched.Remove(optionalProp);
            }

            return unmatched;
        }

        private static void ParseRequiredParameters<TOptions>(string[] args, int offsetInArray, GroupPropertyInfo parameters, TOptions options, ref int currentLogicalPosition) where TOptions : new()
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
                if (!parameters.requiredParam.TryGetValue(currentLogicalPosition, out propInfo))
                {
                    break;
                }

                int paramPosition = currentLogicalPosition; // GetValue changes the current position

                //make sure that we don't run out of array
                if (offsetInArray + currentLogicalPosition >= args.Length)
                {
                    throw new ArgumentException("Required parameters have not been specified");
                }
                var value = GetValueFromArgsArray(args, offsetInArray, ref currentLogicalPosition, propInfo);

                propInfo.SetValue(options, value);
                matchedRequiredParameters++;
            } while (currentLogicalPosition < args.Length);

            // no more? do we have any properties that we have not yet set?
            if (parameters.requiredParam.Count != matchedRequiredParameters)
            {
                throw new ArgumentException("Not all required arguments have been specified");
            }
        }

        private static void ScanTypeForProperties<TOptions>(out TypePropertyInfo tInfo)
        {
            tInfo = new TypePropertyInfo();
            PropertyInfo[] propertiesOnType = typeof(TOptions).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // first of all, find the commandArgument, if any.
            tInfo.ActionCommandProperty = FindCommandProperty(propertiesOnType);

            // parse the rest of the properties
            foreach (var property in propertiesOnType)
            {
                // get the group containing this property (note: more than one group can have the same property)
                // this allows common required parameters

                var groupsWhereThePropertyIs = GetGroupProperty(tInfo, property);

                var actualAttribs = property.GetCustomAttributes<ActualArgumentAttribute>().ToList();
                if (actualAttribs.Count > 1)
                {
                    throw new ArgumentException($"Only one of Required/Optional attribute are allowed per property ({property.Name}). [Red!Help information might be incorrect!]");
                }

                // if we have no attributes on that property, move on
                ActualArgumentAttribute baseAttrib = actualAttribs.FirstOrDefault();
                if (baseAttrib == null)
                {
                    continue;
                }

                // add the property to add the groups it is a part of
                if (baseAttrib is RequiredArgumentAttribute)
                {
                    foreach (GroupPropertyInfo grpPropInfo in groupsWhereThePropertyIs)
                    {
                        if (grpPropInfo.requiredParam.ContainsKey((int)baseAttrib.GetArgumentId()))
                        {
                            throw new ArgumentException("Two required arguments share the same position!!");
                        }

                        grpPropInfo.requiredParam[(int)baseAttrib.GetArgumentId()] = property;
                    }
                }
                else if (baseAttrib is OptionalArgumentAttribute)
                {
                    foreach (GroupPropertyInfo grpPropInfo in groupsWhereThePropertyIs)
                    {

                        if (grpPropInfo.optionalParam.ContainsKey((string)baseAttrib.GetArgumentId()))
                        {
                            throw new ArgumentException("Two optional arguments share the same name!!");
                        }

                        grpPropInfo.optionalParam[(string)baseAttrib.GetArgumentId()] = property;
                    }
                }
            }

            GroupPropertyInfo grp;
            // remove the empty one, if empty
            if (tInfo.TypeInfo.TryGetValue(string.Empty, out grp))
            {
                if (grp.optionalParam.Count == 0 && grp.requiredParam.Count == 0)
                    tInfo.TypeInfo.Remove(string.Empty);
            }
        }

        private static List<GroupPropertyInfo> GetGroupProperty(TypePropertyInfo tInfo, PropertyInfo property)
        {
            List<GroupPropertyInfo> groupsForThisProperty = new List<GroupPropertyInfo>();

            var customAttributes = property.GetCustomAttributes<CommandGroupArgumentAttribute>();

            if (!customAttributes.Any())
            {
                // if we don't have groups defined
                GroupPropertyInfo grpPropInfo;
                if (!tInfo.TypeInfo.TryGetValue(string.Empty, out grpPropInfo))
                {
                    grpPropInfo = new GroupPropertyInfo();
                    tInfo.TypeInfo[string.Empty] = grpPropInfo;
                }

                groupsForThisProperty.Add(grpPropInfo);
                return groupsForThisProperty;
            }

            foreach (var commandGroup in customAttributes)
            {
                string grpPropInfoName = commandGroup?.Name ?? string.Empty;
                GroupPropertyInfo grpPropInfo;
                if (!tInfo.TypeInfo.TryGetValue(grpPropInfoName, out grpPropInfo))
                {
                    grpPropInfo = new GroupPropertyInfo();
                    tInfo.TypeInfo[grpPropInfoName] = grpPropInfo;
                }

                groupsForThisProperty.Add(grpPropInfo);
            }

            // find the group property for this property
            return groupsForThisProperty;
        }

        private static PropertyInfo FindCommandProperty(PropertyInfo[] propertiesOnType)
        {
            PropertyInfo result = null;
            foreach (var prop in propertiesOnType)
            {
                if (prop.GetCustomAttribute<CommandArgumentAttribute>() != null)
                {
                    if (result != null)
                    {
                        throw new ArgumentException($"You can only define a single property as the command property");
                    }
                    result = prop;
                }
            }
            return result;
        }
    }
}

public class TypePropertyInfo
{
    public Dictionary<string, GroupPropertyInfo> TypeInfo { get; } = new Dictionary<string, GroupPropertyInfo>(StringComparer.OrdinalIgnoreCase);
    public PropertyInfo ActionCommandProperty { get; set; }
}

public class GroupPropertyInfo
{
    public Dictionary<int, PropertyInfo> requiredParam { get; } = new Dictionary<int, PropertyInfo>();
    public Dictionary<string, PropertyInfo> optionalParam { get; } = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
}
