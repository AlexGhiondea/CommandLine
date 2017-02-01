using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CommandLine.Analysis
{
    internal class TypeArgumentInfo
    {
        public Dictionary<string, ArgumentGroupInfo> ArgumentGroups { get; } = new Dictionary<string, ArgumentGroupInfo>(StringComparer.OrdinalIgnoreCase);
        public PropertyInfo ActionArgument { get; set; }
    }
}