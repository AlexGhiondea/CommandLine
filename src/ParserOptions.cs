using CommandLine.ColorScheme;
using System;

namespace CommandLine
{
    public class ParserOptions
    {
        public bool ReadFromEnvironment { get; set; } = true;
        public bool LogParseErrorToConsole { get; set; } = true;
        public string VariableNamePrefix { get; set; } = "Commandline_";
    }
}