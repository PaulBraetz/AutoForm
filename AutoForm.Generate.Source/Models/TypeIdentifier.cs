using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
    public readonly struct TypeIdentifier : IEquatable<TypeIdentifier>
    {
        public readonly TypeIdentifierName Name;
        public readonly Namespace Namespace;
        private readonly String _stringRepresentation;

        private TypeIdentifier(TypeIdentifierName name, Namespace @namespace)
        {
            Name = name;
            Namespace = @namespace;

            String namespaceString = String.Concat(Namespace.Parts);
            String nameString = String.Concat(Name.Parts);
            _stringRepresentation = Json.Value(String.IsNullOrEmpty(namespaceString) ? String.IsNullOrEmpty(nameString) ? "null" : nameString.ToString() : $"{namespaceString}.{nameString}");
        }

        public static TypeIdentifier Create(TypeIdentifierName name, Namespace @namespace)
        {
            return new TypeIdentifier(name, @namespace);
        }

        public String ToEscapedString()
        {
            String value = ToString();

            return value == "\"null\""?String.Empty: value.AsSpan(1, value.Length - 2).ToString();
        }

        public override String ToString()
        {
            return _stringRepresentation ?? String.Empty;
        }

        public override Boolean Equals(Object obj)
        {
            return obj is TypeIdentifier identifier && Equals(identifier);
        }

        public Boolean Equals(TypeIdentifier other)
        {
            return _stringRepresentation == other._stringRepresentation;
        }

        public override Int32 GetHashCode()
        {
            return -992964542 + EqualityComparer<String>.Default.GetHashCode(_stringRepresentation);
        }

        public static Boolean operator ==(TypeIdentifier left, TypeIdentifier right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(TypeIdentifier left, TypeIdentifier right)
        {
            return !(left == right);
        }
    }
}
