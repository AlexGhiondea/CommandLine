using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLine.Tests
{
    class Helpers
    {
        public static string[] SplitString(string str)
        {
            return str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static T Parse<T>(string argString) where T : new()
        {
            return CommandLine.Parser.Parse<T>(Helpers.SplitString(argString));
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
