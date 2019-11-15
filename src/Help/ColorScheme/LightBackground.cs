using System;

namespace CommandLine.ColorScheme
{
    public class LightBackground : IColors
    {
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor AssemblyNameColor => ConsoleColor.DarkGray;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.DarkGreen;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.DarkMagenta;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.DarkBlue;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGreen;
    }

    public class GrayBackground : IColors
    {
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor AssemblyNameColor => ConsoleColor.DarkGray;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.DarkGreen;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Magenta;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGreen;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.DarkBlue;
    }
}
