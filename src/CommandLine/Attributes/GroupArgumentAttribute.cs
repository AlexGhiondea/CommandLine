using System;
using System.ComponentModel;

namespace CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public abstract class GroupArgumentAttribute : BaseArgumentAttribute
    {
        public GroupArgumentAttribute(string name, string description) : base(name, description)
        {
        }
    }
}
