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
        public static bool TryParse<TOptions>(string[] args, out TOptions options) where TOptions : new()
        {
            options = default(TOptions);

            TypePropertyInfo parameters = null;
            try
            {
                // build a list of properties for the type passed in.
                // this will throw for cases where the type is incorrecly annotated with attributes
                ScanTypeForProperties<TOptions>(out parameters);

                // short circuit the request for help!
                if (args.Length == 1 && (args[0] == "/?" || args[0] == "-?"))
                {
                    HelpGenerator.DisplayHelp(parameters);
                    return false;
                }

                if (parameters.TypeInfo.ContainsKey(string.Empty))
                {
                    if (parameters.ActionCommandProperty != null)
                    {
                        throw new ArgumentException("Cannot have Command argument unless groups have been specified");
                    }

                    // parse the arguments and build the options object
                    options = InternalParse<TOptions>(args, parameters.TypeInfo[string.Empty]);
                    return true;
                }
                else
                {    // we have groups!
                    if (parameters.ActionCommandProperty == null)
                    {
                        throw new ArgumentException("Cannot have groups unless Command argument has been specified");
                    }

                    string[] remainingArgs = new string[args.Length - 1];
                    Array.Copy(args, 1, remainingArgs, 0, args.Length - 1);


                    // parse based on the command passed in.
                    if (!parameters.TypeInfo.ContainsKey(args[0]))
                    {
                        throw new ArgumentException($"Unknown command [Cyan!{args[0]}]");
                    }
                    options = InternalParse<TOptions>(remainingArgs, parameters.TypeInfo[args[0]]);
                    parameters.ActionCommandProperty.SetValue(options, Convert.ChangeType(GetValueForProperty(args[0], parameters.ActionCommandProperty), parameters.ActionCommandProperty.PropertyType));
                }

                return true;
            }
            catch (Exception ex)
            {
                Colorizer.WriteLine($"[Red!Error]: {ex.Message} {Environment.NewLine}");

                HelpGenerator.DisplayHelp(parameters);

                return false;
            }
        }

        public static TOptions Parse<TOptions>(string[] args) where TOptions : new()
        {
            TOptions options = default(TOptions);
            TryParse(args, out options);
            return options;
        }

        public static object GetValueForProperty(string value, PropertyInfo targetType)
        {
            if (targetType.PropertyType.IsEnum)
            {
                return Enum.Parse(targetType.PropertyType, value);
            }

            return value;
        }

        public static object GetValue(string[] args, ref int currentPosition, PropertyInfo targetType)
        {
            var value = targetType.GetCustomAttribute<ActualArgumentAttribute>();
            object argValue = null;

            if (!value.IsCollection)
            {
                argValue = GetValueForProperty(args[currentPosition], targetType);
                currentPosition++;
            }
            else
            {
                // we are going to support just string lists.
                int indexLastEntry = args.Length;
                for (int i = currentPosition; i < args.Length; i++)
                {
                    if (args[i][0] == '-')
                    {
                        // stop at the first additional argument
                        indexLastEntry = i;
                        break;
                    }
                }

                // create the list
                string[] list = new string[indexLastEntry - currentPosition];
                Array.Copy(args, currentPosition, list, 0, indexLastEntry - currentPosition);

                argValue = new List<string>(list);
                currentPosition = indexLastEntry;
            }

            return Convert.ChangeType(argValue, targetType.PropertyType);
        }

        private static TOptions InternalParse<TOptions>(string[] args, GroupPropertyInfo parameters) where TOptions : new()
        {
            TOptions options = new TOptions();
            int currentPosition = 0;

            // short circuit the request for help!
            if (args.Length == 1 && (args[0] == "/?" || args[0] == "-?"))
            {
                HelpGenerator.DisplayParamterHelpForGroup("", parameters);
                return options;
            }

            // let's match them to actual required args, in positional
            if (parameters.requiredParam.Count > 0)
            {
                if (args.Length == 0)
                {
                    throw new ArgumentException("Required parameters have not been specified");
                }

                do
                {
                    //set the required property
                    PropertyInfo propInfo;
                    if (!parameters.requiredParam.TryGetValue(currentPosition, out propInfo))
                    {
                        break;
                    }

                    int paramPosition = currentPosition; // GetValue changes the current position
                    var value = GetValue(args, ref currentPosition, propInfo);

                    propInfo.SetValue(options, value);
                    parameters.requiredParam.Remove(paramPosition);
                } while (currentPosition < args.Length);
            }

            // no more? do we have any properties that we have not yet set?
            if (parameters.requiredParam.Count > 0)
            {
                throw new ArgumentException("Not all required arguments have been specified");
            }

            // at this point, we should have no more required parameters that have not been set
            Debug.Assert(parameters.requiredParam.Count == 0, "All required parameters should have been set.");

            // process the optional arguments
            while (currentPosition < args.Length)
            {
                if (args[currentPosition][0] != '-')
                {
                    throw new ArgumentException("Optional parameter name should start with '-'");
                }

                PropertyInfo optionalProp = null;
                var optionalParamName = args[currentPosition].Substring(1);
                if (!parameters.optionalParam.TryGetValue(optionalParamName, out optionalProp))
                {
                    throw new ArgumentException($"Could not find argument {args[currentPosition]}");
                }

                // skip over the parameter name
                currentPosition++;

                var value = GetValue(args, ref currentPosition, optionalProp);

                optionalProp.SetValue(options, value);
                parameters.optionalParam.Remove(optionalParamName);
            }

            // for all the remaining optional properties, set their default value.
            foreach (var property in parameters.optionalParam.Values)
            {
                //get the default value..
                var value = property.GetCustomAttribute<OptionalArgumentAttribute>();
                property.SetValue(options, Convert.ChangeType(value.DefaultValue, property.PropertyType));
            }

            return options;
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
                // get the group containing this property
                GroupPropertyInfo grpPropInfo = GetGroupProperty(tInfo, property);

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

                if (baseAttrib is RequiredArgumentAttribute)
                {
                    if (grpPropInfo.requiredParam.ContainsKey((int)baseAttrib.GetArgumentId()))
                    {
                        throw new ArgumentException("Two required arguments share the same position!!");
                    }

                    grpPropInfo.requiredParam[(int)baseAttrib.GetArgumentId()] = property;
                }
                else if (baseAttrib is OptionalArgumentAttribute)
                {
                    if (grpPropInfo.optionalParam.ContainsKey((string)baseAttrib.GetArgumentId()))
                    {
                        throw new ArgumentException("Two optional arguments share the same name!!");
                    }

                    grpPropInfo.optionalParam[(string)baseAttrib.GetArgumentId()] = property;
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

        private static GroupPropertyInfo GetGroupProperty(TypePropertyInfo tInfo, PropertyInfo property)
        {
            // find the group property for this property
            string grpPropInfoName = property.GetCustomAttribute<CommandGroupArgumentAttribute>()?.Name ?? string.Empty;

            GroupPropertyInfo grpPropInfo;
            if (!tInfo.TypeInfo.TryGetValue(grpPropInfoName, out grpPropInfo))
            {
                grpPropInfo = new GroupPropertyInfo();
                tInfo.TypeInfo[grpPropInfoName] = grpPropInfo;
            }

            return grpPropInfo;
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
}
