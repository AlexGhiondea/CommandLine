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

    public class DarkYellowBackgroundColors : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.Green;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Cyan;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.Yellow;
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGray;
    }

    public class GreenBackgroundColors : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.DarkGreen;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Blue;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.DarkRed;
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGray;
    }

    public class CyanBackgroundColors : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.DarkGreen;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Blue;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.DarkRed;
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGray;
    }

        public class RedBackgroundColors : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.Green;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Cyan;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.Yellow;
        public ConsoleColor ErrorColor => ConsoleColor.DarkRed;
        public ConsoleColor ArgumentValueColor => ConsoleColor.Green;
    }
}