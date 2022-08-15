using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Models
{
	public readonly struct Namespace : IEquatable<Namespace>
	{
		private Namespace(IEnumerable<IdentifierPart> parts)
		{
			Parts = parts;
			_stringRepresentation = Json.Value(String.Concat(Parts));
		}

		public static readonly Namespace Attributes = Create().Append("AutoForm").Append("Attributes");
		public static readonly Namespace Generate = Create().Append("AutoForm").Append("Generate");

		public readonly IEnumerable<IdentifierPart> Parts;
		private readonly String _stringRepresentation;

		public static Namespace Create()
		{
			return new Namespace(Array.Empty<IdentifierPart>());
		}
		public Namespace Append(String name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				return this;
			}

			IEnumerable<IdentifierPart> parts = GetNextParts()
				.Append(IdentifierPart.Name(name));

			return new Namespace(parts);
		}
		public Namespace Prepend(String name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				return this;
			}

			IEnumerable<IdentifierPart> parts = GetPreviousParts()
				.Prepend(IdentifierPart.Name(name));

			return new Namespace(parts);
		}
		public Namespace PrependRange(IEnumerable<String> names)
		{
			Namespace @namespace = this;
			foreach (String name in names)
			{
				@namespace = @namespace.Prepend(name);
			}

			return @namespace;
		}
		public Namespace AppendRange(IEnumerable<String> names)
		{
			Namespace @namespace = this;
			foreach (String name in names)
			{
				@namespace = @namespace.Append(name);
			}

			return @namespace;
		}

		private IEnumerable<IdentifierPart> GetNextParts()
		{
			IEnumerable<IdentifierPart> parts = Parts ?? Array.Empty<IdentifierPart>();

			IdentifierPart.PartKind lastKind = parts.LastOrDefault().Kind;

			Boolean prependSeparator = lastKind == IdentifierPart.PartKind.Name;

			return prependSeparator ?
				parts.Append(IdentifierPart.Period()) :
				parts;
		}
		private IEnumerable<IdentifierPart> GetPreviousParts()
		{
			IEnumerable<IdentifierPart> parts = Parts ?? Array.Empty<IdentifierPart>();

			IdentifierPart.PartKind firstKind = parts.FirstOrDefault().Kind;

			Boolean appendSeparator = firstKind == IdentifierPart.PartKind.Name;

			return appendSeparator ?
				parts.Prepend(IdentifierPart.Period()) :
				parts;
		}

		public override String ToString()
		{
			return _stringRepresentation ?? String.Empty;
		}

		public override Boolean Equals(Object obj)
		{
			return obj is Namespace @namespace && Equals(@namespace);
		}

		public Boolean Equals(Namespace other)
		{
			return _stringRepresentation == other._stringRepresentation;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_stringRepresentation);
		}

		public static Boolean operator ==(Namespace left, Namespace right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(Namespace left, Namespace right)
		{
			return !(left == right);
		}
	}
}
