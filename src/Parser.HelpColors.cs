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
        private static IColors s_helpColors = Colors.GetDefault();

        public static class Colors
        {
            public static IColors GetDefault()
            {
                if (
                    Console.BackgroundColor == ConsoleColor.DarkYellow ||
                    Console.BackgroundColor == ConsoleColor.White ||
                    Console.BackgroundColor == ConsoleColor.Yellow || 
                    Console.BackgroundColor == ConsoleColor.Gray)
                {
                    return new LightBackgroundColors();
                }

                return new DarkBackgroundColors();
            }

            public static void Set(IColors colors)
            {
                s_helpColors = colors;
            }
        }
    }
}