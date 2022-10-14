using AutoForm.Analysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Json.Analysis
{
	internal readonly struct PropertyIdentifierJsonDecorator : IJsonDecorator<PropertyIdentifier>, IEquatable<IJson>
	{
		public PropertyIdentifier Value { get; }

		public PropertyIdentifierJsonDecorator(PropertyIdentifier value) : this()
		{
			Value = value;
			_json = Analysis.Json.Value(value.ToString());
		}

		private readonly String _json;
		public String Json => _json ?? "null";
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

		public static Boolean operator ==(PropertyIdentifierJsonDecorator left, PropertyIdentifierJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(PropertyIdentifierJsonDecorator left, PropertyIdentifierJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
