using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
	public readonly struct Template : IEquatable<Template>
	{
		public readonly TypeIdentifier Identifier;
		public readonly TypeIdentifier ModelType;
		private readonly String _stringRepresentation;

		private Template(TypeIdentifier identifier, TypeIdentifier modelType)
		{
			Identifier = identifier;
			ModelType = modelType;

			_stringRepresentation = String.IsNullOrEmpty(Identifier.ToString()) || String.IsNullOrEmpty(ModelType.ToString()) ?
									String.Empty :
									$"[AutoForm.Attributes.AutoControlTemplate(typeof({ModelType}))]\n{Identifier}";
		}

		public static Template Create(TypeIdentifier identifier, TypeIdentifier modelType)
		{
			return new Template(identifier, modelType);
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Template template && Equals(template);
		}

		public Boolean Equals(Template other)
		{
			return _stringRepresentation == other._stringRepresentation;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_stringRepresentation);
		}

		public override String ToString()
		{
			return _stringRepresentation;
		}

		public static Boolean operator ==(Template left, Template right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(Template left, Template right)
		{
			return !(left == right);
		}
	}
}
