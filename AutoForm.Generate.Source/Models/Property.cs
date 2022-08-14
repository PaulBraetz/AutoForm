using System;
using System.Collections.Generic;

namespace AutoForm.Generate.Models
{
	public readonly struct Property : IEquatable<Property>
	{
		public readonly Int32 Order;
		public readonly TypeIdentifier Control;
		public readonly PropertyIdentifier Identifier;
		public readonly TypeIdentifier Type;
		private readonly String _stringRepresentation;

		private Property(PropertyIdentifier identifier, TypeIdentifier type, TypeIdentifier control, Int32 order)
		{
			Identifier = identifier;
			Type = type;
			Control = control;
			Order = order;

			_stringRepresentation = String.IsNullOrEmpty(Identifier.ToString()) || String.IsNullOrEmpty(Type.ToString()) ?
									String.Empty :
									$"[AutoForm.Attributes.AutoControlPropertyOrder({Order})]\n{(String.IsNullOrEmpty(Control.ToString()) ? String.Empty : $"[AutoForm.Attributes.AutoControlPropertyControl(typeof({Control}))]\n")}{Type} {Identifier}";
		}
		public static Property Create(PropertyIdentifier identifier, TypeIdentifier type, TypeIdentifier control, Int32 order)
		{
			return new Property(identifier, type, control, order);
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Property property && Equals(property);
		}

		public Boolean Equals(Property other)
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

		public static Boolean operator ==(Property left, Property right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(Property left, Property right)
		{
			return !(left == right);
		}
	}
}
