using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
	public readonly struct TypeIdentifier : IEquatable<TypeIdentifier>
	{
		private TypeIdentifier(TypeIdentifierName identifier, Namespace @namespace)
		{
			Name = identifier;
			Namespace = @namespace;

			_stringRepresentation = String.IsNullOrEmpty(Name.ToString()) ?
									String.Empty :
									String.IsNullOrEmpty(Namespace.ToString()) ?
									Name.ToString() :
									$"{Namespace}.{Name}";
		}

		public readonly TypeIdentifierName Name;
		public readonly Namespace Namespace;
		private readonly String _stringRepresentation;

		public static TypeIdentifier Create(TypeIdentifierName identifier, Namespace @namespace)
		{
			return new TypeIdentifier(identifier, @namespace);
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
