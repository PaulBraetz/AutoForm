using System;
using System.Collections.Generic;

namespace AutoForm.Analysis
{
    internal readonly struct PropertyIdentifier
    {
        public readonly string Name;

        private PropertyIdentifier(string name) : this()
        {
            Name = name;
		}

        public static PropertyIdentifier Create(string name)
        {
            return new PropertyIdentifier(name);
        }
    }
}
