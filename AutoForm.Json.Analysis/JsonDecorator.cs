using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoForm.Json.Analysis
{
	internal readonly struct JsonDecorator<T> : IEquatable<JsonDecorator<T>>
	{
		private JsonDecorator(T value, String json) : this()
		{
			OriginalValue = value;
			Json = json;
		}

		public readonly T OriginalValue;
		public readonly String Json;

		public override String ToString()
		{
			return Json;
		}

		public static JsonDecorator<T> StringValue(T value)
		{
			var valueString = value is String stringValue ?
				$"\"{stringValue.Replace("\"", "\\\"")}\"" :
				value?.ToString() ?? "null";
			var decorator = new JsonDecorator<T>(value, valueString);

			return decorator;
		}
		public static String Value(String value)
		{
			value = value?.Replace("\"", "\\\"");
			return value != null ? $"\"{value}\"" : "null";
		}
		public static String Object(IEnumerable<String> keyValuepairs)
		{
			return $"{{{String.Join(",", keyValuepairs)}}}";
		}


		public static String KeyValuePair<T>(String name, T value)
		{
			var nameString = Value(name);
			var valueString = StringValue(value);
			var kvpString =
			return $"{Value(name)}:{valueString}";
		}
		public static String KeyValuePair<T>(String name, T[] values)
		{
			return KeyValuePair(name, (IEnumerable<T>)values);
		}
		public static String KeyValuePair<T>(String name, IEnumerable<T> values)
		{
			return values != null ? $"{Value(name)}:[{String.Join(",", values.Select(i => StringValue(i)))}]" : $"{Value(name)}:null";
		}

		public static String KeyValuePair(String name, String value)
		{
			return $"{Value(name)}:{Value(value)}";
		}
		public static String KeyValuePair(String name, String[] values)
		{
			return KeyValuePair(name, (IEnumerable<String>)values);
		}
		public static String KeyValuePair(String name, IEnumerable<String> values)
		{
			return values != null ? $"{Value(name)}:[{String.Join(",", values.Select(i => Value(i)))}]" : $"{Value(name)}:null";
		}

		public static String KeyValuePair(String name, Int32 value)
		{
			return $"{Value(name)}:{value}";
		}

		public override Boolean Equals(Object obj)
		{
			return obj is JsonDecorator<T> decorator && Equals(decorator);
		}
		public Boolean Equals(JsonDecorator<T> other)
		{
			return Json == other.Json;
		}
		public override Int32 GetHashCode()
		{
			return 1403951835 + EqualityComparer<String>.Default.GetHashCode(Json);
		}
		public static Boolean operator ==(JsonDecorator<T> left, JsonDecorator<T> right)
		{
			return left.Equals(right);
		}
		public static Boolean operator !=(JsonDecorator<T> left, JsonDecorator<T> right)
		{
			return !(left == right);
		}
	}
}
