using System;
using System.ComponentModel;

namespace CommandLine.Attributes
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public abstract class ActualArgumentAttribute : BaseArgumentAttribute
    {
        public bool IsCollection { get; private set; }

        public ActualArgumentAttribute(string name, string description, bool isCollection = false)
            : base(name, description)
        {
            IsCollection = isCollection;
        }
        public abstract object GetArgumentId();
    }
}