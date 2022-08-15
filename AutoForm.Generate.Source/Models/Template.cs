using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Models
{
	public readonly struct Template : IEquatable<Template>
	{
		public readonly TypeIdentifier Identifier;
		public readonly IEnumerable<TypeIdentifier> ModelIdentifiers;
		private readonly String _stringRepresentation;

		private Template(TypeIdentifier identifier, IEnumerable<TypeIdentifier> modelIdentifiers)
		{
			modelIdentifiers.ThrowOnDuplicate(TypeIdentifierName.AutoControlTemplateAttribute.ToEscapedString());

			Identifier = identifier;
			ModelIdentifiers = modelIdentifiers;

			_stringRepresentation = Json.Object(Json.KeyValuePair(nameof(ModelIdentifiers), ModelIdentifiers),
												Json.KeyValuePair(nameof(Identifier), Identifier));
		}

		public static Template Create(TypeIdentifier identifier)
		{
			return new Template(identifier, Array.Empty<TypeIdentifier>());
		}
		public Template Append(TypeIdentifier modelIdentifier)
		{
			return new Template(Identifier, ModelIdentifiers.Append(modelIdentifier));
		}
		public Template AppendRange(IEnumerable<TypeIdentifier> modelIdentifiers)
		{
			return new Template(Identifier, ModelIdentifiers.AppendRange(modelIdentifiers));
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
