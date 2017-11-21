# CommandLine
CommandLine parser 

## How to define

To use you need to provide a class that is going to hold the options parsed from the command line and use attributes to define the behavior of the parser.

There are 2 kinds of arguments:
 - Required
   - These must be specified
   - They have a specific location where they must be specified
 - Optional
   - They don't have to be specified
   - They have a name that you need to provide on the command line before you provide the value
   - If they are not specified, the object parsed will have the default value specified.'

### Basic example

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

## Advanced scenarios

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
