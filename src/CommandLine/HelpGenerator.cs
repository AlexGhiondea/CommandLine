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
        private static int DisplayCommandLine(Dictionary<int, PropertyInfo> requiredParam, Dictionary<string, PropertyInfo> optionalParam)
        {
            int maxStringSize = 0;
            for (int i = 0; i < requiredParam.Count; i++)
            {
                if (!requiredParam.ContainsKey(i))
                {
                    Colorizer.WriteLine($"{Environment.NewLine}[Red!Error]: Required argument expected at position [Cyan!{i}]. Type declares arguments at position(s) [Cyan!{string.Join(",", requiredParam.Keys.OrderBy(x => x))}]. [Red!Check type definition].");
                    return -1;
                }

                var b = requiredParam[i].GetCustomAttribute<BaseArgumentAttribute>();
                maxStringSize = Math.Max(maxStringSize, b.Name.Length);
                Colorizer.Write("[Cyan!{0}] ", b.Name);
            }

            foreach (var item in optionalParam.Values)
            {
                var b = item.GetCustomAttribute<BaseArgumentAttribute>();
                maxStringSize = Math.Max(maxStringSize, b.Name.Length);
                Colorizer.Write("\\[-[Yellow!{0}] value\\] ", b.Name);
            }

            Colorizer.WriteLine(string.Empty);

            return maxStringSize;
        }

        public static void DisplayHelp<TOptions>(Dictionary<int, PropertyInfo> requiredParam, Dictionary<string, PropertyInfo> optionalParam) where TOptions : new()
        {
            string exeName = Assembly.GetEntryAssembly()?.GetName()?.Name;
            Colorizer.Write("Usage: [White!{0}.exe] ", exeName);

            int maxStringSize = DisplayCommandLine(requiredParam, optionalParam);
            if (maxStringSize < 0)
            {
                return;
            }

            // write out the required parameters
            for (int i = 0; i < requiredParam.Count; i++)
            {
                var b = requiredParam[i].GetCustomAttribute<BaseArgumentAttribute>();
                if (requiredParam[i].PropertyType.IsEnum)
                {
                    Colorizer.WriteLine("  - [Cyan!{0}] : {1} (one of [Cyan!{2}]) [Magenta!(required)]", b.Name.PadRight(maxStringSize), b.Description, GetEnumValuesAsString(requiredParam[i].PropertyType));
                }
                else
                {
                    Colorizer.WriteLine("  - [Cyan!{0}] : {1} [Magenta!(required)]", b.Name.PadRight(maxStringSize), b.Description);
                }
            }

            // write out the optional parameters
            foreach (var item in optionalParam.Values)
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
