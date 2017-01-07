using System;

namespace CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class RequiredArgumentAttribute : BaseArgumentAttribute
    {
        public int ArgumentPosition { get; private set; }

        public RequiredArgumentAttribute(int position, string name, string description, bool isCollection = false) : base(name, description, isCollection)
        {
            ArgumentPosition = position;
        }
    }
}
