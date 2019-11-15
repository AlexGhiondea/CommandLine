using System;

namespace CommandLine.ColorScheme
{
    public class DarkBackground : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.Green;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Cyan;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.Yellow;
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor ArgumentValueColor => ConsoleColor.Green;
    }

    public class DarkYellowBackground : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.Green;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Cyan;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.Yellow;
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGray;
    }

    public class GreenBackground : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.DarkGreen;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Blue;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.DarkRed;
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGray;
    }

    public class CyanBackground : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.DarkGreen;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Blue;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.DarkRed;
        public ConsoleColor ErrorColor => ConsoleColor.Red;
        public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGray;
    }

    public class RedBackground : IColors
    {
        public ConsoleColor AssemblyNameColor => ConsoleColor.White;
        public ConsoleColor ArgumentGroupColor => ConsoleColor.Green;
        public ConsoleColor RequiredArgumentColor => ConsoleColor.Cyan;
        public ConsoleColor OptionalArgumentColor => ConsoleColor.Yellow;
        public ConsoleColor ErrorColor => ConsoleColor.DarkRed;
        public ConsoleColor ArgumentValueColor => ConsoleColor.Green;
    }
}