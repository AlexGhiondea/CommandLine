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

            private static Dictionary<ConsoleColor, IColors> s_colorMap;

            public static IColors Get()
            {
                if (s_helpColors == null)
                {
                    lock (s_lockObj)
                    {
                        if (s_helpColors == null)
                        {
                            s_helpColors = GetDefault(Console.BackgroundColor);
                        }
                    }

                }
                return s_helpColors;
            }

            internal static IColors GetDefault(ConsoleColor backgroundColor)
            {
                // optimize for black background
                if (backgroundColor == ConsoleColor.Black)
                {
                    return new DarkBackgroundColors();
                }

                if (s_colorMap == null)
                {
                    // initialize the dictionary of colors.
                    s_colorMap = new Dictionary<ConsoleColor, IColors>();
                    s_colorMap[ConsoleColor.Black] = new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.Blue] = new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.Cyan] = new CyanBackgroundColors();
                    s_colorMap[ConsoleColor.DarkBlue]= new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.DarkCyan]= new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.DarkGray]= new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.DarkGreen]= new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.DarkMagenta]= new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.DarkRed]= new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.DarkYellow] = new DarkYellowBackgroundColors();
                    s_colorMap[ConsoleColor.Gray] = new GrayBackgroundColors();
                    s_colorMap[ConsoleColor.Green] = new GreenBackgroundColors();
                    s_colorMap[ConsoleColor.Magenta]= new DarkBackgroundColors();
                    s_colorMap[ConsoleColor.Red] = new RedBackgroundColors();
                    s_colorMap[ConsoleColor.White] = new LightBackgroundColors();
                    s_colorMap[ConsoleColor.Yellow] = new LightBackgroundColors();
                }

                return s_colorMap[backgroundColor];
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