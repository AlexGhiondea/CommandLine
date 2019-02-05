using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandLine.Analysis
{
    internal class ArgumentGroupInfo
    {
        public Dictionary<int, PropertyInfo> RequiredArguments { get; } = new Dictionary<int, PropertyInfo>();
        public Dictionary<string, PropertyInfo> OptionalArguments { get; } = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<PropertyInfo, int> OverridePositions { get; } = new Dictionary<PropertyInfo, int>();
    }
}