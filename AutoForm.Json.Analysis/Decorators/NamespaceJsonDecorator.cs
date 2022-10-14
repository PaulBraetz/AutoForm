using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Json.Analysis.Decorators
{
	internal readonly struct NamespaceJsonDecorator : IJsonDecorator<Namespace>, IEquatable<NamespaceJsonDecorator>
	{
		public NamespaceJsonDecorator(Namespace value) : this()
		{
			Value = value;
			_json = Json.Value(value.ToString());
		}

		public Namespace Value { get; }
		private readonly String _json;

		public static readonly NamespaceJsonDecorator Attributes = Namespace.Create()
			.Append("AutoForm")
			.Append("Attributes")
			.AsJson();

		public override Boolean Equals(Object obj)
		{
			return obj is NamespaceJsonDecorator decorator && Equals(decorator);
		}

		public Boolean Equals(NamespaceJsonDecorator other)
		{
			return _json == other._json;
		}

		public override Int32 GetHashCode()
		{
			return 885466328 + EqualityComparer<String>.Default.GetHashCode(_json);
		}

		public static Boolean operator ==(NamespaceJsonDecorator left, NamespaceJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(NamespaceJsonDecorator left, NamespaceJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
