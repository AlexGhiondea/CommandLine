using Xunit;
using System;
using System.Collections.Generic;
using OutputColorizer;
using System.IO;
using System.Reflection;
using CommandLine.ColorScheme;

namespace CommandLine.Tests
{
    class Helpers
    {
        public static string GetTestLocation()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static T Parse<T>(string argString, IOutputWriter writer = null, IColors colors = null, ParserOptions parserOptions = null)
            where T : new()
        {
            if (writer == null)
            {
                writer = new ConsoleWriter();
            }

            if (colors == null)
            {
                colors = new DarkBackground();
            }

            Colorizer.SetupWriter(writer);
            Parser.ColorScheme.Set(colors);
            return Parser.Parse<T>(argString, parserOptions);
        }

        public static void DisplayHelp<T>(HelpFormat helpFormat, IOutputWriter writer = null, IColors colors = null) where T : new()
        {
            if (writer == null)
            {
                writer = new ConsoleWriter();
            }

            if (colors == null)
            {
                colors = new DarkBackground();
            }

            Colorizer.SetupWriter(writer);
            Parser.ColorScheme.Set(colors);
            Parser.DisplayHelp<T>(helpFormat);
        }

        public static void CollectionEquals(List<string> actual, params string[] expected)
        {
            Assert.Equal(actual.Count, expected.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Equal(actual[i], expected[i]);
            }
        }
    }
}
