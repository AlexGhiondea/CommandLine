using Xunit;
using System;
using System.Collections.Generic;
using OutputColorizer;

namespace CommandLine.Tests
{
    class Helpers
    {
        public static string[] SplitString(string str)
        {
            return str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static T Parse<T>(string argString, IOutputWriter writer = null) where T : new()
        {
            if (writer == null)
                writer = new ConsoleWriter();

            Colorizer.SetupWriter(writer);
            return Parser.Parse<T>(Helpers.SplitString(argString));
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
