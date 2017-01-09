using System;

namespace CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class OptionalArgumentAttribute : ActualArgumentAttribute
    {
        public object DefaultValue { get; private set; }

        public OptionalArgumentAttribute(object defaultValue, string name, string description, bool isCollection = false) : base(name, description, isCollection)
        {
            if (isCollection == true && defaultValue != null)
                throw new ArgumentException("Cannot provide default value for collection parameter");

            DefaultValue = defaultValue;
        }
    }
}
