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

        public static void DisplayHelp<TOptions>(GroupPropertyInfo parameters) where TOptions : new()
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.Write("Usage: [White!{0}.exe] ", exeName);

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
