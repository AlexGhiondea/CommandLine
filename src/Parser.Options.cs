using CommandLine.Analysis;
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine.Colors;

namespace CommandLine
{
    public static partial class Parser
    {
        public static class Options
        {
            public static bool WriteHelpOnError { get; set; } = true;
        }
    }
}