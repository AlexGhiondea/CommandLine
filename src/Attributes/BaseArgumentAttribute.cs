using System;
using System.ComponentModel;

namespace CommandLine.Attributes
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public abstract class BaseArgumentAttribute : Attribute
    {
        public string Description { get; private set; }
        public string Name { get; private set; }

        public BaseArgumentAttribute(string name, string description)
        {
            Description = description;
            Name = name;
        }
    }
}
