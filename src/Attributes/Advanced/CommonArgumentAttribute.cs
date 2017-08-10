using System;
using System.ComponentModel;

namespace CommandLine.Attributes.Advanced
{
    /// <summary>
    /// Arguments with this attribute are contained in all of the defined groups
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class CommonArgumentAttribute : GroupAttribute
    {
        public CommonArgumentAttribute()
        {
        }
    }
}
