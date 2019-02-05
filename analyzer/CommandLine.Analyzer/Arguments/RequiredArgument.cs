using System;

namespace CommandLine.Analyzer
{
    internal class RequiredArgument : NamedArgument
    {
        public int Position { get; set; }

        internal RequiredArgument Clone()
        {
            return new RequiredArgument()
            {
                Description = this.Description,
                IsCollection = this.IsCollection,
                Name = this.Name,
                Position = this.Position,
                Symbol = this.Symbol
            };
        }
    }
}
