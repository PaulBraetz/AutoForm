using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		public String Json { get; }

		public override String ToString()
		{
			return Json;
		}

		public static JsonDecorator<T> Null()
		{
			var json = "null";
			var decorator = new JsonDecorator<T>(default, json);

			return decorator;
		}
		public static JsonDecorator<T> String(T value)
		{
			var valueString = value?.ToString()?.Replace("\"", "\\\"").Prepend('\"').Append('\"');
			var json = valueString == null ?
				"null" :
				System.String.Concat(valueString);
			var decorator = new JsonDecorator<T>(value, json);

			return decorator;
		}
		public static JsonDecorator<T> Number(T value)
		{
			var json = value?.ToString() ?? "null";
			var decorator = new JsonDecorator<T>(value, json);

			return decorator;
		}
		public static JsonDecorator<T[]> StringArray(T[] values)
		{
			var json = $"[{System.String.Join(",", values.Select(v => String(v).Json))}]";
			var decorator = new JsonDecorator<T[]>(values, json);

			return decorator;
		}
		public static JsonDecorator<T[]> NumberArray(T[] values)
		{
			var json = $"[{System.String.Join(",", values.Select(v => Number(v).Json))}]";
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
