using System;

namespace CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class RequiredArgumentAttribute : ActualArgumentAttribute
    {
        public int ArgumentPosition { get; private set; }

        public RequiredArgumentAttribute(int position, string name, string description, bool isCollection = false) : base(name, description, isCollection)
        {
            ArgumentPosition = position;
        }

        public override object GetArgumentId() => ArgumentPosition;
    }
}