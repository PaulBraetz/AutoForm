using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
	public readonly struct TypeIdentifier : IEquatable<TypeIdentifier>
	{
		public static readonly TypeIdentifier AutoControlAttribute = Create(TypeIdentifierName.AutoControlAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier AutoControlAttributesProviderAttribute = Create(TypeIdentifierName.AutoControlAttributesProviderAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier AutoControlModelAttribute = Create(TypeIdentifierName.AutoControlModelAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier AutoControlPropertyControlAttribute = Create(TypeIdentifierName.AutoControlPropertyControlAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier AutoControlPropertyExcludeAttribute = Create(TypeIdentifierName.AutoControlPropertyExcludeAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier AutoControlPropertyOrderAttribute = Create(TypeIdentifierName.AutoControlPropertyOrderAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier AutoControlTemplateAttribute = Create(TypeIdentifierName.AutoControlTemplateAttribute, Namespace.Attributes);

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
			return _stringRepresentation?.AsSpan(1, _stringRepresentation.Length - 2).ToString() ?? String.Empty;
		}

		public override String ToString()
		{
			return _stringRepresentation ?? "null";
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
