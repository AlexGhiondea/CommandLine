using CommandLine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine.Analysis
{
    internal static class PropertyHelpers
    {
        public static object GetValueForProperty(string value, PropertyInfo targetType)
        {
            if (TypeHelpers.IsEnum(targetType.PropertyType))
            {
                return Enum.Parse(targetType.PropertyType, value);
            }

            return value;
        }

        public static object GetValueAsType(string value, PropertyInfo pInfo)
        {
            return Convert.ChangeType(GetValueForProperty(value, pInfo), pInfo.PropertyType);
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
                currentLogicalPosition = indexLastEntry - offsetInArray;  // we need  to take into account the offset in the array here.
            }

            return Convert.ChangeType(argValue, targetType.PropertyType);
        }
    }
}
