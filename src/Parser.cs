using CommandLine.Analysis;
using CommandLine.Attributes;
using CommandLine.Attributes.Advanced;
using OutputColorizer;
using System;
using System.Collections;
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
        /// Parse the specified <paramref name="args"/> into the option type <typeparamref name="TOptions"/>
        /// </summary>
        /// <param name="args">An array of strings containing the arguments and their values</param>
        /// <typeparam name="TOptions">The type of the argument values to create</typeparam>
        public static TOptions Parse<TOptions>(string[] args)
            where TOptions : new()
        {
            if (!InternalTryParse(args, out TOptions options, out Exception ex))
            {
                throw ex;
            }

            return options;
        }

        /// <summary>
        /// Parse the specified <paramref name="strArgs"/> string into the option type <typeparamref name="TOptions"/>
        /// </summary>
        /// <param name="strArgs">A string containing the arguments and their values</param>
        /// <typeparam name="TOptions">The type of the argument values to create</typeparam>
        public static TOptions Parse<TOptions>(string strArgs)
            where TOptions : new()
        {
            return Parse<TOptions>(SplitCommandLineIntoSegments(strArgs).ToArray());
        }

        /// <summary>
        /// Try to parse the specified <paramref name="strArgs"/> string into the option type <typeparamref name="TOptions"/>
        /// </summary>
        /// <typeparam name="TOptions">The type of the argument values to create</typeparam>
        /// <param name="strArgs">A string containing the arguments and their values</param>
        /// <param name="options">A variable that will contain the parsed arguments as an object</param>
        /// <returns>True if the parameters were parsed. False if not.</returns>
        public static bool TryParse<TOptions>(string strArgs, out TOptions options)
            where TOptions : new()
        {
            return TryParse(SplitCommandLineIntoSegments(strArgs).ToArray(), out options);
        }

        /// <summary>
        /// Try to parse the specified <paramref name="args"/> string into the option type <typeparamref name="TOptions"/>
        /// </summary>
        /// <typeparam name="TOptions">The type of the argument values to create</typeparam>
        /// <param name="args">An array  containing the arguments and their values</param>
        /// <param name="options">A variable that will contain the parsed arguments as an object</param>
        /// <returns>True if the parameters were parsed. False if not.</returns>
        public static bool TryParse<TOptions>(string[] args, out TOptions options)
            where TOptions : new()
        {
            return InternalTryParse(args, out options, out _);
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
                throw new ParserException($"Unrecognized help format {helpFormat}", null);
            }

            try
            {
                // build a list of properties for the type passed in.
                // this will throw for cases where the type is incorrecly annotated with attributes
                TypeHelpers.ScanTypeForProperties<TOptions>(out TypeArgumentInfo arguments);

                // If we get here, the options type is well defined, so let's display the help.
                HelpGenerator.DisplayHelp(helpFormat, arguments, Configuration.DisplayColors.Get());
            }
            catch (Exception ex)
            {
                string errorFormat = $"[{Configuration.DisplayColors.Get().ErrorColor}!Error]: {{0}} {{1}}";
                // If we were asked to display the help and something went wrong, display the error that went wrong.
                Colorizer.WriteLine(errorFormat, ex.Message, Environment.NewLine);
            }
        }

        private static bool InternalTryParse<TOptions>(string[] args, out TOptions options, out Exception ex)
            where TOptions : new()
        {
            options = default;
            ex = null;
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
                        HelpGenerator.DisplayHelp(HelpFormat.Short, arguments, Configuration.DisplayColors.Get());
                        return true;
                    }
                    else if (args[0] == HelpGenerator.RequestLongHelpParameter)
                    {
                        HelpGenerator.DisplayHelp(HelpFormat.Full, arguments, Configuration.DisplayColors.Get());
                        return true;
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
            catch (Exception innerParserException)
            {
                ex = new ParserException(innerParserException.Message, innerParserException);
                if (Configuration.DisplayErrorMessageOnError)
                {
                    string errorFormat = $"[{Configuration.DisplayColors.Get().ErrorColor}!Error]: {{0}} {{1}}";
                    Colorizer.WriteLine(errorFormat, ex.Message, Environment.NewLine);

                    HelpGenerator.DisplayHelp(HelpFormat.Short, arguments, Configuration.DisplayColors.Get());
                }
                return false;
            }
        }
    }
}
