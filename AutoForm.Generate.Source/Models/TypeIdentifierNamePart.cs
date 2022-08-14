using System;

namespace AutoForm.Generate.Models
{
	public readonly struct IdentifierPart
	{
		public enum PartKind : Byte
		{
			None,
			Array,
			GenericOpen,
			GenericClose,
			Comma,
			Period,
			Name
		}

		private IdentifierPart(String name, PartKind kind)
		{
			Kind = kind;

			switch (Kind)
			{
				case PartKind.Array:
					_stringRepresentation = "[]";
					break;
				case PartKind.GenericOpen:
					_stringRepresentation = "<";
					break;
				case PartKind.GenericClose:
					_stringRepresentation = ">";
					break;
				case PartKind.Period:
					_stringRepresentation = ".";
					break;
				case PartKind.Comma:
					_stringRepresentation = ", ";
					break;
				default:
					_stringRepresentation = name;
					break;
			}
		}
		private IdentifierPart(PartKind kind) : this(null, kind) { }

		public readonly PartKind Kind;
		private readonly String _stringRepresentation;

		public static IdentifierPart Name(String name)
		{
			return new IdentifierPart(name, PartKind.Name);
		}
		public static IdentifierPart Array()
		{
			return new IdentifierPart(PartKind.Array);
		}
		public static IdentifierPart GenericOpen()
		{
			return new IdentifierPart(PartKind.GenericOpen);
		}
		public static IdentifierPart GenericClose()
		{
			return new IdentifierPart(PartKind.GenericClose);
		}
		public static IdentifierPart Period()
		{
			return new IdentifierPart(PartKind.Period);
		}
		public static IdentifierPart Comma()
		{
			return new IdentifierPart(PartKind.Comma);
		}
		public override String ToString()
		{
			return _stringRepresentation ?? String.Empty;
		}
	}
}
