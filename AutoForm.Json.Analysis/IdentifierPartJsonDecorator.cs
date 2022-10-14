using RhoMicro.CodeAnalysis;
using System;
using static RhoMicro.CodeAnalysis.IdentifierPart;
using System.Xml.Linq;
using System.Collections.Generic;

namespace AutoForm.Json.Analysis
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
		public String Json => _json ?? "null";
		public override string ToString()
		{
			return Json;
		}

		public override bool Equals(object obj)
        {
            return obj is IdentifierPartJsonDecorator decorator && Equals(decorator);
        }

        public bool Equals(IdentifierPartJsonDecorator other)
        {
            return _json == other._json;
        }

        public override int GetHashCode()
        {
            return -1954173868 + Value.GetHashCode();
        }

        public static bool operator ==(IdentifierPartJsonDecorator left, IdentifierPartJsonDecorator right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IdentifierPartJsonDecorator left, IdentifierPartJsonDecorator right)
        {
            return !(left == right);
        }
    }
}
