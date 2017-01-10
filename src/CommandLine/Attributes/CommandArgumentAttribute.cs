using System;
using System.ComponentModel;

namespace CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CommandArgumentAttribute : BaseArgumentAttribute
    {
        public CommandArgumentAttribute(string name, string description) : base(name, description)
        {
        }
    }
}
