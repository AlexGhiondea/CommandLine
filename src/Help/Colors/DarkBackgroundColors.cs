using System;

namespace CommandLine.Colors
{
    public class DarkBackgroundColors : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.Green;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Cyan;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.Yellow;
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor ArgumentValueColor => ConsoleColor.Green;
    }
}