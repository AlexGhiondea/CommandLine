using CommandLine.ColorScheme;
using System;

namespace CommandLine
{
    public class ParserOptions
    {
        /// <summary>
        /// Use environment variables when the optional values are not specified on the command line
        /// </summary>
        public bool ReadFromEnvironment { get; set; } = true;

        /// <summary>
        /// Log the error to the console during parsing
        /// </summary>
        public bool LogParseErrorToConsole { get; set; } = true;

        /// <summary>
        /// The prefix to use when reading the environment variables for optional values.
        /// The format it will use to search for the variables is: {VariableNamePrefix}{optionalValueName}
        /// </summary>
        public string VariableNamePrefix { get; set; } = "Commandline_";
    }
}