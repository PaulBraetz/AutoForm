using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
	public readonly struct PropertyIdentifier : IEquatable<PropertyIdentifier>
	{
		public readonly String Name;
		private readonly String _stringRepresentation;

		private PropertyIdentifier(String name) : this()
		{
			Name = name;

			_stringRepresentation = Json.Object(Json.KeyValuePair(nameof(Name), Name));
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
			return _stringRepresentation == other._stringRepresentation;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_stringRepresentation);
		}

		public override String ToString()
		{
			return _stringRepresentation ?? String.Empty;
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
