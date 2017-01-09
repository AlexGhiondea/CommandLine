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

                // parse the arguments and build the options object
                options = InternalParse<TOptions>(args, parameters.TypeInfo[string.Empty]);

                return true;
            }
            catch (Exception ex)
            {
                Colorizer.WriteLine($"[Red!Error]: {ex.Message} {Environment.NewLine}");

                HelpGenerator.DisplayHelp<TOptions>(parameters.TypeInfo[string.Empty]);

                return false;
            }
        }

        public static TOptions Parse<TOptions>(string[] args) where TOptions : new()
        {
            TOptions options = default(TOptions);
            TryParse(args, out options);
            return options;
        }

        public static object GetValue(string[] args, ref int currentPosition, PropertyInfo targetType)
        {
            var value = targetType.GetCustomAttribute<ActualArgumentAttribute>();
            object argValue = null;

            if (!value.IsCollection)
            {
                if (targetType.PropertyType.IsEnum)
                {
                    argValue = Enum.Parse(targetType.PropertyType, args[currentPosition]);
                }
                else
                {
                    argValue = args[currentPosition];
                }
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
            // short circuit the request for help!
            if (args.Length == 1 && (args[0] == "/?" || args[0] == "-?"))
            {
                HelpGenerator.DisplayHelp<TOptions>(parameters);
                return default(TOptions);
            }

            TOptions options = new TOptions();
            int currentPosition = 0;

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

            GroupPropertyInfo propertyInfo = null;

            // retrieve all the arguments defined on the class
            var allProps = typeof(TOptions).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in allProps)
            {
                var actualAttribs = property.GetCustomAttributes<ActualArgumentAttribute>().ToList();
                if (actualAttribs.Count > 1)
                {
                    throw new ArgumentException($"Only one of Required/Optional attribute are allowed per property ({property.Name}). [Red!Help information might be incorrect!]");
                }

                string propertyGroupName = string.Empty; // if no group is defined (at all!) use empty string
                var groupAttrib = property.GetCustomAttribute<GroupArgumentAttribute>();
                if (groupAttrib != null)
                {
                    propertyGroupName = groupAttrib.Name;
                }

                // have we seen this group before?
                if (!tInfo.TypeInfo.TryGetValue(propertyGroupName, out propertyInfo))
                {
                    propertyInfo = new GroupPropertyInfo();
                    tInfo.TypeInfo[propertyGroupName] = propertyInfo;
                }

                // if we have no attributes on that property, move on
                ActualArgumentAttribute baseAttrib = actualAttribs.FirstOrDefault();
                if (baseAttrib == null)
                {
                    continue;
                }

                if (baseAttrib is RequiredArgumentAttribute)
                {
                    var reqArg = baseAttrib as RequiredArgumentAttribute;
                    if (propertyInfo.requiredParam.ContainsKey(reqArg.ArgumentPosition))
                    {
                        throw new ArgumentException("Two required arguments share the same position!!");
                    }

                    propertyInfo.requiredParam[reqArg.ArgumentPosition] = property;
                }
                else if (baseAttrib is OptionalArgumentAttribute)
                {
                    var optArg = baseAttrib as OptionalArgumentAttribute;
                    if (propertyInfo.optionalParam.ContainsKey(optArg.Name))
                    {
                        throw new ArgumentException("Two optional arguments share the same name!!");
                    }

                    propertyInfo.optionalParam[optArg.Name] = property;
                }
            }
        }
    }

    public class TypePropertyInfo
    {
        public Dictionary<string, GroupPropertyInfo> TypeInfo { get; } = new Dictionary<string, GroupPropertyInfo>(StringComparer.OrdinalIgnoreCase);
    }

    public class GroupPropertyInfo
    {
        public Dictionary<int, PropertyInfo> requiredParam { get; } = new Dictionary<int, PropertyInfo>();
        public Dictionary<string, PropertyInfo> optionalParam { get; } = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
    }
}
