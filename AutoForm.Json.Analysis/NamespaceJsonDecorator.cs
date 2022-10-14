using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Json.Analysis
{
	internal readonly struct NamespaceJsonDecorator : IJsonDecorator<Namespace>, IEquatable<IJson>
	{
		public NamespaceJsonDecorator(Namespace value) : this()
		{
			Value = value;
			_json = Analysis.Json.Value(value.ToString());
		}

		private readonly String _json;
		public String Json => _json ?? "null";

		public Namespace Value { get; }

		public override string ToString()
		{
			return Json;
		}

		public override Boolean Equals(Object obj)
		{
			return obj is IJson json && Equals(json);
		}

		public Boolean Equals(IJson other)
		{
			return Json == other.Json;
		}

		public override Int32 GetHashCode()
		{
			return 885466328 + Json.GetHashCode();
		}

		public static Boolean operator ==(ErrorJsonDecorator left, ErrorJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(ErrorJsonDecorator left, ErrorJsonDecorator right)
		{
			return !(left == right);
		}
		public static readonly IJsonDecorator<Namespace> Attributes = Namespace.Create()
			.Append("AutoForm")
			.Append("Attributes")
			.AsJson();
	}
}
