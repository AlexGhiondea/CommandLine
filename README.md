# CommandLine
CommandLine parser 

[![NuGet version](https://img.shields.io/nuget/v/commandline.net.svg?style=flat)](https://www.nuget.org/packages/CommandLine.Net/)
[![Nuget downloads](https://img.shields.io/nuget/dt/commandline.net.svg?style=flat)](https://www.nuget.org/packages/CommandLine.Net/)
![Build And Test](https://github.com/AlexGhiondea/CommandLine/workflows/Build%20And%20Test/badge.svg)
[![codecov](https://codecov.io/gh/AlexGhiondea/CommandLine/branch/master/graph/badge.svg)](https://codecov.io/gh/AlexGhiondea/CommandLine)
[![MIT License](https://img.shields.io/github/license/AlexGhiondea/CommandLine.svg)](https://github.com/AlexGhiondea/CommandLine/blob/master/LICENSE)
[![Code Factor](https://www.codefactor.io/repository/github/alexghiondea/commandline/badge)](https://www.codefactor.io/repository/github/alexghiondea/commandline)
========

## Getting started

Here is how you can define a class that can be used with the parser:

```csharp
class Options
{
    [RequiredArgument(0,"dir","Directory")]
    public string Directory { get; set; }

    [OptionalArgument("*.*","pattern", "Search pattern")]
    public string SearchPattern { get; set; }
}
```

This is the code that you need to parse the command line arguments into an object:

```csharp
if (!Parser.TryParse(args, out Options options))
{
    return;
}
```

The parser understands common requests for help as you can see below.

## Display the available commands

The parser understand the following help commands:
 - `-?` which is the short form help
 
 ```
Usage:
 <exename> dir [-pattern value]

For detailed information run '<exename> --help'.
 ```
 
 - `--help` which is the long form help
 
 ```
Usage:
 <exename> dir [-pattern value]
  - dir     : Directory (string, required)
  - pattern : Search pattern (string, default=*.*)
 ```
### Display the help using code

If you want to display the help manually, you can use the `Parser.DisplayHelp()` API to do it:
```csharp
Parser.DisplayHelp<Options>();
```

By default, the API assumes you are using the `HelpFormat.Short` specifier which maps to `/?` or `-?`.

You can ask for the long version of the help by using the `HelpFormat.Full` specifier:

```csharp
Parser.DisplayHelp<Options>(HelpFormat.Full);
```

## Types of arguments 

To use you need to provide a class that is going to hold the options parsed from the command line and use attributes to define the behavior of the parser.

There are 2 kinds of arguments:
 - Required
   - These must be specified
   - They have a specific location where they must be specified
 - Optional
   - They don't have to be specified
   - They have a name that you need to provide on the command line before you provide the value
   - If they are not specified, the object parsed will have the default value specified.'

## Advanced scenarios

### Argument grouping
There are cases when you want to have different sets of arguments depending on one of the arguments.

The argument that will be used as the differentiator will be marked with the `ActionArgument` attribute. That argument must be specified.

You can then define multiple groups of arguments. 
You will specify the argument group for a property using the `ArgumentGroup` attribute. 
A single property in a class can take part in many argument groups.

If a property will be common to all groups, you can use the `CommonArgument' attribute

```csharp
internal class CommandLineOptions
{
    [ActionArgument]
    public CommandLineActionGroup Action { get; set; }

    [CommonArgument]
    [RequiredArgumentAttribute(0, "host", "The name of the host")]
    public string Host { get; set; }

    #region Add-host Action
    [RequiredArgument(1, "mac", "The MAC address of the host")]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    public string MAC { get; set; }

    #endregion
}
```

If you have a required argument that is shared between multiple argument groups but use different positions, you can tweak the expected position depending on the group

```csharp
internal class CommandLineOptions
{
    [ActionArgument]
    public CommandLineActionGroup Action { get; set; }

    // Here the argument 'name' is expected at position 1 in the add group and position 0 in the list group
    [ArgumentGroup(nameof(CommandLineActionGroup.list), 0)]
    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    [RequiredArgumentAttribute(1, "name", "The name of the host")]
    public string Host { get; set; }

    [ArgumentGroup(nameof(CommandLineActionGroup.add))]
    [RequiredArgumentAttribute(0, "host", "The name of the host")]
    public string Host { get; set; }

}
```

### Argument position override

Sometimes you want to have a common parameter shared across multiple commands, but you want the position of it to be different in those two groups.

To do that, you use the constructor for the `ArgumentGroup` attribute that takes as the second argument the override position for that argument group.

In the example below, the common parameter `repos` is defined as required in position `1`.
For the group `Create` where no override position is specified, the parameter will be required at position `1`.
For the group `List` where the override position is specified, the parameter will be required at position `0`.

```csharp
class OverridePositionGroup
{
    [ActionArgument]
    public Action Action { get; set; }

    [ArgumentGroup(nameof(Action.Create))]
    [RequiredArgument(0, "milestoneInputFile", "The file containing the list of milestones to create.")]
    public string MilestoneFile { get; set; }

    [ArgumentGroup(nameof(Action.List), 0)]
    [ArgumentGroup(nameof(Action.Create))]
    [RequiredArgument(1, "repos", "The list of repositories where to add the milestones to. The format is: owner\\repoName.", true)]
    public List<string> Repositories { get; set; }
}
```

### Background color

The parser will automatically detect the color to use when displaying help in a command prompt. There are a set of colors that are predefined depending on the console color.

You can specify different colors to be used by implementing the `IColor` interface and configuring the parser:

```csharp
public class CustomColors : IColors
{
    public ConsoleColor ErrorColor => ConsoleColor.Red;
    public ConsoleColor AssemblyNameColor => ConsoleColor.DarkGray;
    public ConsoleColor ArgumentGroupColor => ConsoleColor.DarkGreen;
    public ConsoleColor RequiredArgumentColor => ConsoleColor.Magenta;
    public ConsoleColor ArgumentValueColor => ConsoleColor.DarkGreen;
    public ConsoleColor OptionalArgumentColor => ConsoleColor.DarkBlue;
}

// Configure the parser to use the new colors
Parser.Configuration.DisplayColors.Set(new CustomColors());
```

### Environment variable parsing

By default, the parser will try and infer optional parameters from the environment variables if they are not specified in the command line.

The order of precedence is going to be (from most specific to least specific):
1. Values passed in via the command line
2. Values specified in the environment
3. Default value specified in the argument type

By default, the Parser will look for environment variables with this name:

```
CommandLine_<name>
```

The `<name>` represents the optional parameter name as defined in the type declaration.

You can change the prefix for the Parser:
```csharp
ParserOptions parserOptions = new ParserOptions() { VariableNamePrefix = "myPrefix" };

if (!Parser.TryParse(args, out Options options, parserOptions))
{
    return;
}
```

You can also disable the feature completely:
```csharp
ParserOptions parserOptions = new ParserOptions() { ReadFromEnvironment = false };

if (!Parser.TryParse(args, out Options options, parserOptions))
{
    return;
}
```
