using System;
using System.ComponentModel;

namespace CommandLine.Attributes.Advanced
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class GroupAttribute : Attribute
    {
        public GroupAttribute()
        {
        }
    }
}
