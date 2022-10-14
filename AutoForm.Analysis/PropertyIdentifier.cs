using System;
using System.Collections.Generic;

namespace AutoForm.Analysis
{
    internal readonly struct PropertyIdentifier : IEquatable<PropertyIdentifier>
    {
        public readonly string Name;
        private readonly string _string;

        private PropertyIdentifier(string name) : this()
        {
            Name = name;
            _string = Name;
		}

        public override string ToString()
        {
            return _string;
        }

        public static PropertyIdentifier Create(string name)
        {
            return new PropertyIdentifier(name);
        }

        public override Boolean Equals(Object obj)
        {
            return obj is PropertyIdentifier identifier && Equals(identifier);
        }

        public Boolean Equals(PropertyIdentifier other)
        {
            return _string == other._string;
        }

        public override Int32 GetHashCode()
        {
            return -219028617 + EqualityComparer<String>.Default.GetHashCode(_string);
        }

        public static Boolean operator ==(PropertyIdentifier left, PropertyIdentifier right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(PropertyIdentifier left, PropertyIdentifier right)
        {
            return !(left == right);
        }
    }
}
