﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Json.Analysis
{
	internal readonly struct JsonDecorator<T> : IEquatable<JsonDecorator<T>>, IJson
	{
		private JsonDecorator(T value, String json) : this()
		{
			OriginalValue = value;
			Json = json;
		}

		public readonly T OriginalValue;
		public String Json {
			get;
		}

		public override String ToString() => Json;

		public static JsonDecorator<T> Null()
		{
			var json = "null";
			var decorator = new JsonDecorator<T>(default, json);

			return decorator;
		}
		public static JsonDecorator<T> String(T value, Func<T, String> converter = null)
		{
			var valueString = (converter?.Invoke(value) ?? value?.ToString())?.Replace("\"", "\\\"").Prepend('\"').Append('\"');
			var json = valueString == null ?
				"null" :
				System.String.Concat(valueString);
			var decorator = new JsonDecorator<T>(value, json);

			return decorator;
		}
		public static JsonDecorator<T> Number(T value, Func<T, String> converter = null)
		{
			var json = converter?.Invoke(value) ?? value?.ToString() ?? "null";
			var decorator = new JsonDecorator<T>(value, json);

			return decorator;
		}
		public static JsonDecorator<T[]> StringArray(T[] values, Func<T, String> converter = null)
		{
			var json = $"[{System.String.Join(",", values.Select(v => String(v, converter).Json))}]";
			var decorator = new JsonDecorator<T[]>(values, json);

			return decorator;
		}
		public static JsonDecorator<T[]> NumberArray(T[] values, Func<T, String> converter = null)
		{
			var json = $"[{System.String.Join(",", values.Select(v => Number(v, converter).Json))}]";
			var decorator = new JsonDecorator<T[]>(values, json);

			return decorator;
		}
		public static JsonDecorator<T> KeyValuePair(String key, JsonDecorator<T> decoratedValue)
		{
			var json = $"{JsonDecorator<String>.String(key)}:{decoratedValue}";
			var decorator = new JsonDecorator<T>(decoratedValue.OriginalValue, json);

			return decorator;
		}
		public static JsonDecorator<T> Object(T value, Func<T, IJson[]> memberFactory)
		{
			var json = value != null ?
				$"{{{System.String.Join(",", memberFactory.Invoke(value).Select(kvp => kvp.Json))}}}" :
				"null";
			var decorator = new JsonDecorator<T>(value, json);

			return decorator;
		}
		public static JsonDecorator<T[]> ObjectArray(T[] values, Func<T, IJson[]> memberFactory)
		{
			var json = $"[{System.String.Join(",", values.Select(v => Object(v, memberFactory).Json))}]";
			var decorator = new JsonDecorator<T[]>(values, json);

			return decorator;
		}

#pragma warning disable
		public override Boolean Equals(Object obj)
		{
			return obj is JsonDecorator<T> decorator && Equals(decorator);
		}
#pragma warning restore

		public Boolean Equals(JsonDecorator<T> other) => Json == other.Json;
		public override Int32 GetHashCode() => 1403951835 + EqualityComparer<String>.Default.GetHashCode(Json);
		public static Boolean operator ==(JsonDecorator<T> left, JsonDecorator<T> right) => left.Equals(right);
		public static Boolean operator !=(JsonDecorator<T> left, JsonDecorator<T> right) => !(left == right);
	}
}