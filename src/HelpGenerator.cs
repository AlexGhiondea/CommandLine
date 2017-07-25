﻿using CommandLine.Analysis;
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

        internal static void DisplayHelp(string helpFormat, TypeArgumentInfo arguments)
        {
            if (helpFormat == "/?" || helpFormat == "-?")
            {
                DisplayShortHelp(arguments);

            }
            else if (helpFormat == "--help")
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
                    Colorizer.WriteLine("  - [Cyan!{0}] : {1} (one of [Green!{2}]) [Magenta!(required)]", b.Name.PadRight(maxStringSize), b.Description, GetEnumValuesAsString(arguments.RequiredArguments[i].PropertyType));
                }
                else
                {
                    Colorizer.WriteLine("  - [Cyan!{0}] : {1} [Magenta!(required)]", b.Name.PadRight(maxStringSize), b.Description);
                }
            }

            // write out the optional arguments
            foreach (var item in arguments.OptionalArguments.Values)
            {
                var b = item.GetCustomAttribute<OptionalArgumentAttribute>();
                if (TypeHelpers.IsEnum(item.PropertyType))
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