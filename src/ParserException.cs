using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLine
{
    public class ParserException : Exception
    {
        public ParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
