using System;
using System.ComponentModel;

namespace CommandLine.Attributes.Advanced
{
    /// <summary>
    /// Use this attribute to distinguish a property that will be used to decide how to parse
    /// the following arguments.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ActionArgumentAttribute : Attribute
    {
        public ActionArgumentAttribute()
        {
        }
    }
}
