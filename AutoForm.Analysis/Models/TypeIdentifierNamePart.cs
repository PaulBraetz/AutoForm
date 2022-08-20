using System;

namespace AutoForm.Analysis.Models
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

		public readonly PartKind Kind;
		private readonly String _json;
		private readonly String _string;

		private IdentifierPart(String name, PartKind kind)
		{
			Kind = kind;

			switch (Kind)
			{
				case PartKind.Array:
					_json = "[]";
					break;
				case PartKind.GenericOpen:
					_json = "<";
					break;
				case PartKind.GenericClose:
					_json = ">";
					break;
				case PartKind.Period:
					_json = ".";
					break;
				case PartKind.Comma:
					_json = ", ";
					break;
				default:
					_json = name;
					break;
			}

			_string = _json;
		}
		private IdentifierPart(PartKind kind) : this(null, kind) { }

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
			return _json ?? "null";
		}
		public String ToEscapedString()
		{
			return _string ?? String.Empty;
		}
	}
}
