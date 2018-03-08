using CommandLine.Analysis;
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
        public const string RequestLongHelpParameter = "--help";

        private static int DisplayCommandLine(ArgumentGroupInfo arguments)
        {
            int maxStringSize = 0;
            for (int i = 0; i < arguments.RequiredArguments.Count; i++)
            {
                if (!arguments.RequiredArguments.ContainsKey(i))
                {
                    Colorizer.WriteLine($"{Environment.NewLine}[Red!Error]: Required argument expected at position [Cyan!{i}]. Type declares arguments at position(s) [Cyan!{string.Join(",", arguments.RequiredArguments.Keys.OrderBy(x => x))}]. [Red!Check type definition].");
                    return -1;
                }

                var b = arguments.RequiredArguments[i].GetCustomAttribute<ActualArgumentAttribute>();
                maxStringSize = Math.Max(maxStringSize, b.Name.Length);
                Colorizer.Write("[Cyan!{0}] ", b.Name);
            }

            foreach (var item in arguments.OptionalArguments.Values)
            {
                var b = item.GetCustomAttribute<ActualArgumentAttribute>();
                maxStringSize = Math.Max(maxStringSize, b.Name.Length);
                Colorizer.Write("\\[-[Yellow!{0}] value\\] ", b.Name);
            }

            Colorizer.WriteLine(string.Empty);

            return maxStringSize;
        }

        internal static void DisplayHelp(HelpFormat helpFormat, TypeArgumentInfo arguments)
        {
            switch (helpFormat)
            {
                case HelpFormat.Short:
                    DisplayShortHelp(arguments);
                    break;
                case HelpFormat.Full:
                    DisplayDetailedHelp(arguments);
                    break;
                default:
                    throw new ArgumentException("Unrecognized help format", nameof(helpFormat));
            }
        }

        private static void DisplayHelp(string helpFormat, TypeArgumentInfo arguments)
        {
            if (helpFormat == RequestShortHelpParameter || helpFormat == "/?")
            {
                DisplayShortHelp(arguments);

            }
            else if (helpFormat == RequestLongHelpParameter)
            {
                DisplayDetailedHelp(arguments);
            }
        }

        private static void DisplayShortHelp(TypeArgumentInfo type)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            DisplayCommandLine(exeName, type);

            Colorizer.WriteLine(string.Empty);
            Colorizer.WriteLine("For detailed information run '[White!{0} --help]'.", exeName);
        }

        private static void DisplayDetailedHelp(TypeArgumentInfo type)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            foreach (var item in type.ArgumentGroups.Keys)
            {
                DisplayDetailedArgumentHelp(exeName, item, type.ArgumentGroups[item]);
            }
        }

        public static void DisplayHelpForCommmand(string command, ArgumentGroupInfo propertyGroup)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            DisplayDetailedArgumentHelp(exeName, command, propertyGroup);
        }

        private static void DisplayCommandLine(string exeName, TypeArgumentInfo type)
        {
            foreach (var group in type.ArgumentGroups)
            {
                Colorizer.Write(" [White!{0}.exe] ", exeName);
                if (!string.IsNullOrEmpty(group.Key))
                {
                    Colorizer.Write($"[Green!{group.Key}] ");
                }
                DisplayCommandLine(group.Value);
            }
        }

        private static void DisplayDetailedArgumentHelp(string exeName, string command, ArgumentGroupInfo arguments)
        {
            Colorizer.Write(" [White!{0}.exe] ", exeName);
            if (!string.IsNullOrEmpty(command))
            {
                Colorizer.Write("[Green!{0}] ", command);
            }
            DisplayDetailedParameterHelp(arguments);
        }

        private static void DisplayDetailedParameterHelp(ArgumentGroupInfo arguments)
        {
            int maxStringSize = DisplayCommandLine(arguments);
            if (maxStringSize < 0)
            {
                return;
            }

            // write out the required arguments
            for (int i = 0; i < arguments.RequiredArguments.Count; i++)
            {
                var b = arguments.RequiredArguments[i].GetCustomAttribute<ActualArgumentAttribute>();
                if (TypeHelpers.IsEnum(arguments.RequiredArguments[i].PropertyType))
                {
                    Colorizer.WriteLine("  - [Cyan!{0}] : {1} (one of [Green!{2}], [Cyan!required])", b.Name.PadRight(maxStringSize), b.Description, GetEnumValuesAsString(arguments.RequiredArguments[i].PropertyType));
                }
                else
                {
                    Colorizer.WriteLine("  - [Cyan!{0}] : {1} ([Green!{2}], [Cyan!required])", b.Name.PadRight(maxStringSize), b.Description, GetFriendlyTypeName(arguments.RequiredArguments[i].PropertyType));
                }
            }

            // write out the optional arguments
            foreach (var item in arguments.OptionalArguments.Values)
            {
                var b = item.GetCustomAttribute<OptionalArgumentAttribute>();

                if (TypeHelpers.IsEnum(item.PropertyType))
                {
                    Colorizer.WriteLine("  - [Yellow!{0}] : {1} (one of [Green!{2}], default=[Yellow!{3}])", b.Name.PadRight(maxStringSize), b.Description, GetEnumValuesAsString(item.PropertyType), b.DefaultValue);
                }
                else
                {
                    Colorizer.WriteLine("  - [Yellow!{0}] : {1} ([Green!{3}], default=[Yellow!{2}])", b.Name.PadRight(maxStringSize), b.Description, b.DefaultValue ?? "", GetFriendlyTypeName(item.PropertyType));
                }
            }
            Colorizer.WriteLine(string.Empty);
        }

        private static string GetEnumValuesAsString(Type enumType)
        {
            return string.Join(",", Enum.GetNames(enumType));
        }

        private static string GetFriendlyTypeName(Type propertyType)
        {
            if (propertyType == typeof(string))
                return "string";

            if (propertyType == typeof(int) || propertyType == typeof(uint) ||
                propertyType == typeof(sbyte) || propertyType == typeof(byte) ||
                propertyType == typeof(short) || propertyType == typeof(ushort) ||
                propertyType == typeof(double) || propertyType == typeof(float) ||
                propertyType == typeof(long) || propertyType == typeof(ulong))
                return "number";

            if (propertyType == typeof(bool))
                return "true or false";

            if (propertyType == typeof(char))
                return "char";

            if (TypeHelpers.IsEnum(propertyType))
                return "enum";

            if (TypeHelpers.IsList(propertyType))
                return "list";

            return "";
        }
    }
}
