using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
    public readonly struct PropertyIdentifier : IEquatable<PropertyIdentifier>
    {
        public readonly String Name;
        private readonly String _json;
        private readonly String _string;

        private PropertyIdentifier(String name) : this()
        {
            Name = name;

            _string = Name;
            _json = Json.Value(_string);
        }

        public override String ToString()
        {
            return _json ?? "null";
        }
        public String ToEscapedString()
        {
            return _string ?? String.Empty;
        }

        public static PropertyIdentifier Create(String name)
        {
            return new PropertyIdentifier(name);
        }

        public override Boolean Equals(Object obj)
        {
            return obj is PropertyIdentifier identifier && Equals(identifier);
        }

        public Boolean Equals(PropertyIdentifier other)
        {
            return _json == other._json;
        }

        public override Int32 GetHashCode()
        {
            return -992964542 + EqualityComparer<String>.Default.GetHashCode(_json);
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
