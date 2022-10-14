using AutoForm.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoForm.Json.Analysis
{
	internal readonly struct ErrorJsonDecorator : IJsonDecorator<Error>, IEquatable<IJson>
	{
		public Error Value { get; }

		public ErrorJsonDecorator(Error value)
		{
			Value = value;
			_json = Analysis.Json.Object(Analysis.Json.KeyValuePair(nameof(value.Exceptions), value.Exceptions.Select(m => m.Message)));
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

		public static Boolean operator ==(ErrorJsonDecorator left, ErrorJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(ErrorJsonDecorator left, ErrorJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
