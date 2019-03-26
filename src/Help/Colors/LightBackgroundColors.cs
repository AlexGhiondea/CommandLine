using System;

namespace CommandLine.Colors
{
    public class LightBackgroundColors : IColors
    {
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor AssemblyNameColor => ConsoleColor.DarkGray;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.DarkGreen;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.DarkMagenta;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.DarkBlue;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGreen;
    }

}
