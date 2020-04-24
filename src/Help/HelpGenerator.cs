using CommandLine.Analysis;
using CommandLine.Attributes;
using OutputColorizer;
using System;
using System.Linq;
using System.Reflection;
using CommandLine.ColorScheme;

namespace CommandLine
{
    internal static class HelpGenerator
    {
        public const string RequestShortHelpParameter = "-?";
        public const string RequestLongHelpParameter = "--help";

        internal static void DisplayHelp(HelpFormat helpFormat, TypeArgumentInfo arguments, IColors colors)
        {
            switch (helpFormat)
            {
                case HelpFormat.Short:
                    DisplayShortHelp(arguments, colors);
                    break;
                case HelpFormat.Full:
                    DisplayDetailedHelp(arguments, colors);
                    break;
            }
        }

        private static void DisplayShortHelp(TypeArgumentInfo type, IColors colors)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            DisplayCommandLine(exeName, type, colors);

            Colorizer.WriteLine(string.Empty);
            string errorFormat = $"For detailed information run '[{colors.AssemblyNameColor}!{{0}} --help]'.";
            Colorizer.WriteLine(errorFormat, exeName);
        }

        private static void DisplayDetailedHelp(TypeArgumentInfo type, IColors colors)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            foreach (var item in type.ArgumentGroups.Keys)
            {
                DisplayDetailedArgumentHelp(exeName, item, type.ArgumentGroups[item], colors);
            }
        }

        public static void DisplayHelpForCommmand(string command, ArgumentGroupInfo propertyGroup, IColors colors)
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.WriteLine("Usage: ");

            DisplayDetailedArgumentHelp(exeName, command, propertyGroup, colors);
        }

        private static void DisplayCommandLine(string exeName, TypeArgumentInfo type, IColors colors)
        {
            foreach (var group in type.ArgumentGroups)
            {
                string assemblyNameFormat = $" [{colors.AssemblyNameColor}!{{0}}.exe] "; // " [White!{{0}}.exe] "
                Colorizer.Write(assemblyNameFormat, exeName);
                if (!string.IsNullOrEmpty(group.Key))
                {
                    string groupFormat = $" [{colors.ArgumentGroupColor}!{{0}}] "; // " [Green!{{0}}] "
                    Colorizer.Write(string.Format(groupFormat, group.Key));
                }
                DisplayCommandLine(group.Value, colors);
            }
        }

        private static int DisplayCommandLine(ArgumentGroupInfo arguments, IColors colors)
        {
            int maxStringSize = 0;
            for (int i = 0; i < arguments.RequiredArguments.Count; i++)
            {
                if (!arguments.RequiredArguments.ContainsKey(i))
                {
                    string requiredArgError = $"{Environment.NewLine}[{colors.ErrorColor}!Error]: Required argument expected at position[{colors.RequiredArgumentColor}!{{0}}]. Type declares arguments at position(s) [{colors.RequiredArgumentColor}!{{1}}]. [{colors.ErrorColor}!Check type definition].";

                    Colorizer.WriteLine(requiredArgError, i, string.Join(",", arguments.RequiredArguments.Keys.OrderBy(x => x)));
                    return -1;
                }

                var b = arguments.RequiredArguments[i].GetCustomAttribute<ActualArgumentAttribute>();
                maxStringSize = Math.Max(maxStringSize, b.Name.Length);

                string argumentName = $"[{colors.RequiredArgumentColor}!{{0}}] ";
                Colorizer.Write(argumentName, b.Name);
            }

            foreach (var item in arguments.OptionalArguments.Values)
            {
                var b = item.GetCustomAttribute<ActualArgumentAttribute>();
                maxStringSize = Math.Max(maxStringSize, b.Name.Length);

                string optionalArgumentFormat = $"\\[-[{colors.OptionalArgumentColor}!{{0}}] value\\] ";
                Colorizer.Write(optionalArgumentFormat, b.Name);
            }

            Colorizer.WriteLine(string.Empty);

            return maxStringSize;
        }

        private static void DisplayDetailedArgumentHelp(string exeName, string command, ArgumentGroupInfo arguments, IColors colors)
        {
            string argumentNameFormat = $" [{colors.AssemblyNameColor}!{{ 0}}.exe] ";
            Colorizer.Write(argumentNameFormat, exeName);
            if (!string.IsNullOrEmpty(command))
            {
                string formatArgs = $"[{colors.ArgumentGroupColor}!{{0}}] ";
                Colorizer.Write(formatArgs, command);
            }
            DisplayDetailedParameterHelp(arguments, colors);
        }

        private static void DisplayDetailedParameterHelp(ArgumentGroupInfo arguments, IColors colors)
        {
            int maxStringSize = DisplayCommandLine(arguments, colors);

            // write out the required arguments
            for (int i = 0; i < arguments.RequiredArguments.Count; i++)
            {
                var b = arguments.RequiredArguments[i].GetCustomAttribute<ActualArgumentAttribute>();
                if (TypeHelpers.IsEnum(arguments.RequiredArguments[i].PropertyType))
                {
                    string requiredArgFormatDetailed = $"  - [{colors.RequiredArgumentColor}!{{0}}] : {{1}} (one of [{colors.ArgumentValueColor}!{{2}}], [{colors.RequiredArgumentColor}!required])";
                    Colorizer.WriteLine(requiredArgFormatDetailed, b.Name.PadRight(maxStringSize), b.Description, GetEnumValuesAsString(arguments.RequiredArguments[i].PropertyType));
                }
                else
                {
                    string requiredArgFormatDetailed = $"  - [{colors.RequiredArgumentColor}!{{0}}] : {{1}} ([{colors.ArgumentValueColor}!{{2}}], [{colors.RequiredArgumentColor}!required])";
                    Colorizer.WriteLine(requiredArgFormatDetailed, b.Name.PadRight(maxStringSize), b.Description, GetFriendlyTypeName(arguments.RequiredArguments[i].PropertyType));
                }
            }

            // write out the optional arguments
            foreach (var item in arguments.OptionalArguments.Values)
            {
                var b = item.GetCustomAttribute<OptionalArgumentAttribute>();

                if (TypeHelpers.IsEnum(item.PropertyType))
                {
                    string optionalFormatArg = $"  - [{colors.OptionalArgumentColor}!{{0}}] : {{1}} (one of [{colors.ArgumentValueColor}!{{2}}], default=[{colors.OptionalArgumentColor}!{{3}}])";
                    Colorizer.WriteLine(optionalFormatArg, b.Name.PadRight(maxStringSize), b.Description, GetEnumValuesAsString(item.PropertyType), b.DefaultValue);
                }
                else
                {
                    string optionalFormatArg = $"  - [{colors.OptionalArgumentColor}!{{0}}] : {{1}} ([{colors.ArgumentValueColor}!{{3}}], default=[{colors.OptionalArgumentColor}!{{2}}])";
                    Colorizer.WriteLine(optionalFormatArg, b.Name.PadRight(maxStringSize), b.Description, b.DefaultValue ?? "", GetFriendlyTypeName(item.PropertyType));
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
            {
                return "string";
            }

            if (propertyType == typeof(int) || propertyType == typeof(uint) ||
                propertyType == typeof(sbyte) || propertyType == typeof(byte) ||
                propertyType == typeof(short) || propertyType == typeof(ushort) ||
                propertyType == typeof(double) || propertyType == typeof(float) ||
                propertyType == typeof(long) || propertyType == typeof(ulong))
            {
                return "number";
            }

            if (propertyType == typeof(bool))
            {
                return "true or false";
            }

            if (propertyType == typeof(char))
            {
                return "char";
            }

            if (TypeHelpers.IsList(propertyType))
            {
                return "list";
            }

            return "";
        }
    }
}
