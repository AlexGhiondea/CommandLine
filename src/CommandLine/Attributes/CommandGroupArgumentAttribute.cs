﻿using System;
using System.ComponentModel;

namespace CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CommandGroupArgumentAttribute : Attribute
    {
        public string Name { get; private set; }
        public CommandGroupArgumentAttribute(string name)
        {
            Name = name;
        }
    }
}