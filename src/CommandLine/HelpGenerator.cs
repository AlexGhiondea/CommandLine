using CommandLine.Attributes;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLine
{
    internal class HelpGenerator
    {
        public const string RequestShortHelpParameter = "-?";

        private static int DisplayCommandLine(GroupPropertyInfo parameters)
        {
            int maxStringSize = 0;
            for (int i = 0; i < parameters.requiredParam.Count; i++)
            {
                if (!parameters.requiredParam.ContainsKey(i))
                {
                    Colorizer.WriteLine($"{Environment.NewLine}[Red!Error]: Required argument expected at position [Cyan!{i}]. Type declares arguments at position(s) [Cyan!{string.Join(",", parameters.requiredParam.Keys.OrderBy(x => x))}]. [Red!Check type definition].");
                    return -1;
                }

                var b = parameters.requiredParam[i].GetCustomAttribute<ActualArgumentAttribute>();
                maxStringSize = Math.Max(maxStringSize, b.Name.Length);
                Colorizer.Write("[Cyan!{0}] ", b.Name);
            }

            foreach (var item in parameters.optionalParam.Values)
            {
                var b = item.GetCustomAttribute<ActualArgumentAttribute>();
                maxStringSize = Math.Max(maxStringSize, b.Name.Length);
                Colorizer.Write("\\[-[Yellow!{0}] value\\] ", b.Name);
            }

            Colorizer.WriteLine(string.Empty);

            return maxStringSize;
        }

        internal static void DisplayHelp(string helpFormat, TypePropertyInfo parameters)
        {
            if (helpFormat == "/?" || helpFormat == "-?")
            {
                DisplayShortHelp(parameters);
                
            }
            else if (helpFormat == "--help")
            {
                DisplayDetailedHelp(parameters);
            }
        }

        private static void DisplayShortHelp(TypePropertyInfo type)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            DisplayCommandLine(exeName, type);

            Colorizer.WriteLine(string.Empty);
            Colorizer.WriteLine("For detailed information run '[White!{0} --help]'.", exeName);
        }

        private static void DisplayDetailedHelp(TypePropertyInfo type)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            foreach (var item in type.TypeInfo.Keys)
            {
                DisplayDetailedParameterHelp(exeName, item, type.TypeInfo[item]);
            }
        }

        public static void DisplayHelpForCommmand(string command, GroupPropertyInfo propertyGroup)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            DisplayDetailedParameterHelp(exeName, command, propertyGroup);
        }

        private static void DisplayCommandLine(string exeName, TypePropertyInfo type)
        {
            foreach (var group in type.TypeInfo)
            {
                Colorizer.Write(" [White!{0}.exe] ", exeName);
                if (!string.IsNullOrEmpty(group.Key))
                {
                    Colorizer.Write($"[Green!{group.Key}] ");
                }
                DisplayCommandLine(group.Value);
            }
        }

        private static void DisplayDetailedParameterHelp(string exeName, string command, GroupPropertyInfo parameters)
        {
            Colorizer.Write(" [White!{0}.exe] ", exeName);
            if (!string.IsNullOrEmpty(command))
            {
                Colorizer.Write("[Green!{0}] ", command);
            }
            DisplayDetailedParameterHelp(parameters);
        }

        private static void DisplayDetailedParameterHelp(GroupPropertyInfo parameters)
        {
            int maxStringSize = DisplayCommandLine(parameters);
            if (maxStringSize < 0)
            {
                return;
            }

            // write out the required parameters
            for (int i = 0; i < parameters.requiredParam.Count; i++)
            {
                var b = parameters.requiredParam[i].GetCustomAttribute<ActualArgumentAttribute>();
                if (parameters.requiredParam[i].PropertyType.IsEnum)
                {
                    Colorizer.WriteLine("  - [Cyan!{0}] : {1} (one of [Cyan!{2}]) [Magenta!(required)]", b.Name.PadRight(maxStringSize), b.Description, GetEnumValuesAsString(parameters.requiredParam[i].PropertyType));
                }
                else
                {
                    Colorizer.WriteLine("  - [Cyan!{0}] : {1} [Magenta!(required)]", b.Name.PadRight(maxStringSize), b.Description);
                }
            }

            // write out the optional parameters
            foreach (var item in parameters.optionalParam.Values)
            {
                var b = item.GetCustomAttribute<OptionalArgumentAttribute>();
                if (item.PropertyType.IsEnum)
                {
                    Colorizer.WriteLine("  - [Yellow!{0}] : {1} (one of [Cyan!{2}]) (default=[Green!{3}])", b.Name.PadRight(maxStringSize), b.Description, GetEnumValuesAsString(item.PropertyType), b.DefaultValue);
                }
                else
                {
                    Colorizer.WriteLine("  - [Yellow!{0}] : {1} (default=[Green!{2}])", b.Name.PadRight(maxStringSize), b.Description, b.DefaultValue ?? "");
                }
            }
            Colorizer.WriteLine(string.Empty);
        }

        private static string GetEnumValuesAsString(Type enumType)
        {
            return string.Join(",", Enum.GetNames(enumType));
        }
    }
}
