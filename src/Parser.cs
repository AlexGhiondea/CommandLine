using CommandLine.Analysis;
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CommandLine
{
    public static partial class Parser
    {
        /// <summary>
        /// The default IColors object will automatically detect the color of the background and will
        /// use the appropriate colors when generating the help
        /// </summary>
        public static TOptions Parse<TOptions>(string[] args)
            where TOptions : new()
        {
            TryParse(args, out TOptions options);
            return options;
        }

        public static TOptions Parse<TOptions>(string strArgs)
            where TOptions : new()
        {
            List<string> args = new List<string>();
            SplitCommandLineIntoSegments(strArgs, ref args);
            TryParse(args.ToArray(), out TOptions options);
            return options;
        }

        public static bool TryParse<TOptions>(string strArgs, out TOptions options)
            where TOptions : new()
        {
            List<string> args = new List<string>();
            SplitCommandLineIntoSegments(strArgs, ref args);
            return TryParse(args.ToArray(), out options);
        }

        public static bool TryParse<TOptions>(string[] args, out TOptions options)
            where TOptions : new()
        {
            options = default(TOptions);

            TypeArgumentInfo arguments = null;
            try
            {
                // build a list of properties for the type passed in.
                // this will throw for cases where the type is incorrecly annotated with attributes
                TypeHelpers.ScanTypeForProperties<TOptions>(out arguments);

                // before we do anything, let's expand the response files (if any).
                args = ExpandResponseFiles(args);

                // short circuit the request for help!
                if (args.Length == 1)
                {
                    if (args[0] == HelpGenerator.RequestShortHelpParameter || args[0] == "/?")
                    {
                        HelpGenerator.DisplayHelp(HelpFormat.Short, arguments, Colors.Get());
                        return false;
                    }
                    else if (args[0] == HelpGenerator.RequestLongHelpParameter)
                    {
                        HelpGenerator.DisplayHelp(HelpFormat.Full, arguments, Colors.Get());
                        return false;
                    }
                }

                // we have groups!
                if (!arguments.ArgumentGroups.ContainsKey(string.Empty))
                {
                    return ParseCommandGroups(args, ref options, arguments);
                }

                // parse the arguments and build the options object
                options = InternalParse<TOptions>(args, 0, arguments.ArgumentGroups[string.Empty]);
                return true;
            }
            catch (Exception ex)
            {
                string errorFormat = $"[{Colors.Get().ErrorColor}!Error]: {{0}} {{1}}";
                Colorizer.WriteLine(errorFormat, ex.Message, Environment.NewLine);

                HelpGenerator.DisplayHelp(HelpFormat.Short, arguments, Colors.Get());

                return false;
            }
        }

        /// <summary>
        /// Display the help based on the <typeparamref name="TOptions"/> type provided
        /// </summary>
        /// <typeparam name="TOptions">The type for which to generate the help.</typeparam>
        /// <param name="helpFormat">Describes the level of details to generate for the help message.</param>
        public static void DisplayHelp<TOptions>(HelpFormat helpFormat = HelpFormat.Short)
        {
            if (helpFormat != HelpFormat.Short && helpFormat != HelpFormat.Full)
            {
                throw new ArgumentException("Unrecognized help format", nameof(helpFormat));
            }

            try
            {
                // build a list of properties for the type passed in.
                // this will throw for cases where the type is incorrecly annotated with attributes
                TypeHelpers.ScanTypeForProperties<TOptions>(out TypeArgumentInfo arguments);

                // If we get here, the options type is well defined, so let's display the help.
                HelpGenerator.DisplayHelp(helpFormat, arguments, Colors.Get());
            }
            catch (Exception ex)
            {
                string errorFormat = $"[{Colors.Get().ErrorColor}!Error]: {{0}} {{1}}";
                // If we were asked to display the help and something went wrong, display the error that went wrong.
                Colorizer.WriteLine(errorFormat, ex.Message, Environment.NewLine);
            }
        }
    }
}
