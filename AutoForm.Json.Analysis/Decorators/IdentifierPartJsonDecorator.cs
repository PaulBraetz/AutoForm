using RhoMicro.CodeAnalysis;
using System;
using static RhoMicro.CodeAnalysis.IdentifierPart;
using System.Xml.Linq;
using System.Collections.Generic;

namespace AutoForm.Json.Analysis.Decorators
{
	internal readonly struct IdentifierPartJsonDecorator : IEquatable<IdentifierPartJsonDecorator>, IJsonDecorator<IdentifierPart>
	{
		public IdentifierPart Value { get; }

		public IdentifierPartJsonDecorator(IdentifierPart part) : this()
		{
			Value = part;
			_json = part.ToString();
		}

		private readonly String _json;

		public override String ToString()
		{
			return _json ?? "null";
		}

		public override Boolean Equals(Object obj)
		{
			return obj is IdentifierPartJsonDecorator decorator && Equals(decorator);
		}

		public Boolean Equals(IdentifierPartJsonDecorator other)
		{
			return _json == other._json;
		}

		public override Int32 GetHashCode()
		{
			return -1954173868 + Value.GetHashCode();
		}

		public static Boolean operator ==(IdentifierPartJsonDecorator left, IdentifierPartJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(IdentifierPartJsonDecorator left, IdentifierPartJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
