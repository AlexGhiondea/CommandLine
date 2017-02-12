using CommandLine.Attributes;
using System;
using System.Collections.Generic;

namespace CommandLine.Tests
{
    internal class HelpGeneratorObject
    {
        [OptionalArgument(UniqueProviders.FileSize, "provider", "The mechanism to use when determining if the files are unique")]
        public UniqueProviders Provider { get; set; }

        [RequiredArgument(0, "folders", "List of the folders to consider when scanning for duplicates", true)]
        public List<string> RootFolders { get; set; }

        [RequiredArgument(1, "providers", "Some providers to have")]
        public UniqueProviders Providers { get; set; }

        [OptionalArgument("output", "out", "The name of the file where to write the result")]
        public string ReportFileName { get; set; }

        [OptionalArgument(true, "open", "Launch the result once the tool runs")]
        public bool LaunchReport { get; set; }

        [OptionalArgument(OutputProviders.Csv | OutputProviders.Html, "outputWriter", "The output format(s) to use")]
        public OutputProviders OutputGenerator { get; set; }
    }

    internal enum UniqueProviders
    {
        SHA1,
        FileSize
    }

    [Flags]
    internal enum OutputProviders
    {
        Html = 1,
        Csv = 2
    }
}
