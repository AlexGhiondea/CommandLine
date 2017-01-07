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

            Dictionary<int, PropertyInfo> requiredParam = null;
            Dictionary<string, PropertyInfo> optionalParam = null;
            try
            {
                // build a list of properties for the type passed in.
                // this will throw for cases where the type is incorrecly annotated with attributes
                ScanTypeForProperties<TOptions>(out requiredParam, out optionalParam);

                // parse the arguments and build the options object
                options = InternalParse<TOptions>(args, requiredParam, optionalParam);

                return true;
            }
            catch (Exception ex)
            {
                Colorizer.WriteLine($"[Red!Error]: {ex.Message} {Environment.NewLine}");

                HelpGenerator.DisplayHelp<TOptions>(requiredParam, optionalParam);

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
            var value = targetType.GetCustomAttribute<BaseArgumentAttribute>();
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

        private static TOptions InternalParse<TOptions>(string[] args, Dictionary<int, PropertyInfo> requiredParam, Dictionary<string, PropertyInfo> optionalParam) where TOptions : new()
        {
            // short circuit the request for help!
            if (args.Length == 1 && (args[0] == "/?" || args[0] == "-?"))
            {
                HelpGenerator.DisplayHelp<TOptions>(requiredParam, optionalParam);
                return default(TOptions);
            }

            TOptions options = new TOptions();
            int currentPosition = 0;

            // let's match them to actual required args, in positional
            if (requiredParam.Count > 0)
            {
                if (args.Length == 0)
                {
                    throw new ArgumentException("Required parameters have not been specified");
                }

                do
                {
                    //set the required property
                    PropertyInfo propInfo;
                    if (!requiredParam.TryGetValue(currentPosition, out propInfo))
                    {
                        break;
                    }

                    int paramPosition = currentPosition; // GetValue changes the current position
                    var value = GetValue(args, ref currentPosition, propInfo);

                    propInfo.SetValue(options, value);
                    requiredParam.Remove(paramPosition);
                } while (currentPosition < args.Length);
            }

            // no more? do we have any properties that we have not yet set?
            if (requiredParam.Count > 0)
            {
                throw new ArgumentException("Not all required arguments have been specified");
            }

            // at this point, we should have no more required parameters that have not been set
            Debug.Assert(requiredParam.Count == 0, "All required parameters should have been set.");

            // process the optional arguments
            while (currentPosition < args.Length)
            {
                if (args[currentPosition][0] != '-')
                {
                    throw new ArgumentException("Optional parameter name should start with '-'");
                }

                PropertyInfo optionalProp = null;
                var optionalParamName = args[currentPosition].Substring(1);
                if (!optionalParam.TryGetValue(optionalParamName, out optionalProp))
                {
                    throw new ArgumentException($"Could not find argument {args[currentPosition]}");
                }

                // skip over the parameter name
                currentPosition++;

                var value = GetValue(args, ref currentPosition, optionalProp);

                optionalProp.SetValue(options, value);
                optionalParam.Remove(optionalParamName);
            }

            // for all the remaining optional properties, set their default value.
            foreach (var property in optionalParam.Values)
            {
                //get the default value..
                var value = property.GetCustomAttribute<OptionalArgumentAttribute>();
                property.SetValue(options, Convert.ChangeType(value.DefaultValue, property.PropertyType));
            }

            return options;
        }

        private static void ScanTypeForProperties<TOptions>(out Dictionary<int, PropertyInfo> requiredParam, out Dictionary<string, PropertyInfo> optionalParam)
        {
            requiredParam = new Dictionary<int, PropertyInfo>();
            optionalParam = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            // retrieve all the arguments defined on the class
            var allProps = typeof(TOptions).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in allProps)
            {
                var attrs = property.GetCustomAttributes<BaseArgumentAttribute>().ToList();

                if (attrs.Count > 1)
                {
                    throw new ArgumentException($"Only one of Required/Optional attribute are allowed per property ({property.Name}). [Red!Help information might be incorrect!]");
                }

                // if we have no attributes on that property, move on
                BaseArgumentAttribute baseAttrib = attrs.FirstOrDefault();
                if (baseAttrib == null)
                {
                    continue;
                }

                if (baseAttrib is RequiredArgumentAttribute)
                {
                    var reqArg = baseAttrib as RequiredArgumentAttribute;
                    if (requiredParam.ContainsKey(reqArg.ArgumentPosition))
                    {
                        throw new ArgumentException("Two required arguments share the same position!!");
                    }

                    requiredParam[reqArg.ArgumentPosition] = property;
                }
                else if (baseAttrib is OptionalArgumentAttribute)
                {
                    var optArg = baseAttrib as OptionalArgumentAttribute;
                    if (optionalParam.ContainsKey(optArg.Name))
                    {
                        throw new ArgumentException("Two optional arguments share the same name!!");
                    }

                    optionalParam[optArg.Name] = property;
                }
            }
        }
    }
}
