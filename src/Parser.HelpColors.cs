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
        public static class Colors
        {
            private static IColors s_helpColors;
            private static object s_lockObj = new object();
            public static IColors Get()
            {
                if (s_helpColors == null)
                {
                    lock (s_lockObj)
                    {
                        if (s_helpColors == null)
                        {
                            s_helpColors = GetDefault();
                        }
                    }

                }
                return s_helpColors;
            }

            internal static IColors GetDefault()
            {
                if (Console.BackgroundColor == ConsoleColor.Black)
                {
                    return new DarkBackgroundColors();
                }

                if (Console.BackgroundColor == ConsoleColor.Gray)
                {
                    return new GrayBackgroundColors();
                }
                else if (Console.BackgroundColor == ConsoleColor.DarkYellow)
                {
                    return new DarkYellowBackgroundColors();
                }
                else if (Console.BackgroundColor == ConsoleColor.Green)
                {
                    return new GreenBackgroundColors();
                }
                else if (Console.BackgroundColor == ConsoleColor.Cyan)
                {
                    return new CyanBackgroundColors();
                }
                else if (Console.BackgroundColor == ConsoleColor.Red)
                {
                    return new RedBackgroundColors();
                }
                else if (
                    Console.BackgroundColor == ConsoleColor.White ||
                    Console.BackgroundColor == ConsoleColor.Yellow)
                {
                    return new LightBackgroundColors();
                }

                return new DarkBackgroundColors();
            }
            public static void Set(IColors colors)
            {
                lock (s_lockObj)
                {
                    s_helpColors = colors;
                }
            }
        }
    }
}