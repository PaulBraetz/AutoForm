using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Models
{
	public readonly struct Control : IEquatable<Control>
	{
		public readonly TypeIdentifier Identifier;
		public readonly IEnumerable<TypeIdentifier> ModelIdentifiers;
		private readonly String _stringRepresentation;

		private Control(TypeIdentifier identifier, IEnumerable<TypeIdentifier> modelIdentifiers)
		{
			modelIdentifiers.ThrowOnDuplicate(TypeIdentifierName.AutoControlAttribute.ToEscapedString());

			Identifier = identifier;
			ModelIdentifiers = modelIdentifiers;

			_stringRepresentation = Json.Object(Json.KeyValuePair(nameof(ModelIdentifiers), ModelIdentifiers),
												Json.KeyValuePair(nameof(Identifier), Identifier));
		}

		public static Control Create(TypeIdentifier identifier)
		{
			return new Control(identifier, Array.Empty<TypeIdentifier>());
		}
		public Control Append(TypeIdentifier modelIdentifier)
		{
			return new Control(Identifier, ModelIdentifiers.Append(modelIdentifier));
		}
		public Control AppendRange(IEnumerable<TypeIdentifier> modelIdentifiers)
		{
			return new Control(Identifier, ModelIdentifiers.AppendRange(modelIdentifiers));
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Control control && Equals(control);
		}

		public Boolean Equals(Control other)
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

		public static Boolean operator ==(Control left, Control right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(Control left, Control right)
		{
			return !(left == right);
		}
	}
}
