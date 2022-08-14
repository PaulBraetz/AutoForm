using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
	public readonly struct Control : IEquatable<Control>
	{
		public readonly TypeIdentifier ModelIdentifier;
		public readonly TypeIdentifier Identifier;
		private readonly String _stringRepresentation;

		private Control(TypeIdentifier modelIdentifier, TypeIdentifier identifier)
		{
			ModelIdentifier = modelIdentifier;
			Identifier = identifier;

			_stringRepresentation = String.IsNullOrEmpty(ModelIdentifier.ToString()) || String.IsNullOrEmpty(Identifier.ToString()) ?
									String.Empty :
									$"[AutoForm.Attributes.AutoControl(typeof({ModelIdentifier}))]\n{Identifier}";
		}

		public static Control Create(TypeIdentifier identifier, TypeIdentifier modelIdentifier)
		{
			return new Control(modelIdentifier, identifier);
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
