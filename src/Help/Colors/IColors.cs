using System;

namespace CommandLine.Colors
{
    public interface IColors
    {
        ConsoleColor AssemblyNameColor { get; }
        ConsoleColor ArgumentGroupColor { get; }
        ConsoleColor RequiredArgumentColor { get; }
        ConsoleColor OptionalArgumentColor { get; }
        ConsoleColor ErrorColor { get; }
        ConsoleColor ArgumentValueColor { get; }
    }
}