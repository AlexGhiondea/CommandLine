using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using System.Collections.Generic;

namespace CommandLine.Tests.TestObjects
{
    // define required collection property as first one
    class SimpleType3
    {
        /// <summary>
        /// Type of action by files
        /// </summary>
        [RequiredArgument(0, "action", "what need do: convert or unconvert")]
        public ActionType ActionType { get; private set; }
    }

    /// <summary>
    /// Action type
    /// </summary>
    public enum ActionType
    {
        Convert,
        UnConvert
    }
}
