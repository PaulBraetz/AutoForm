﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate
{
	internal static class Json
	{
		public static String Value<T>(T value)
		{
			var valueString = value?.ToString();
			return String.IsNullOrEmpty(valueString) ? "null" : valueString;
		}
		public static String KeyValuePair<T>(String name, T value)
		{
			String valueString = Value(value);
			return $"{Value(name)}:{valueString}";
		}
		public static String KeyValuePair<T>(String name, IEnumerable<T> values)
		{
			return values != null ? $"{Value(name)}:[{String.Join(",", values.Select(i => Value(i)))}]" : $"{Value(name)}:null";
		}

		public static String Value(String value)
		{
			value = value?.Replace("\"", "\\\"");
			return value != null ? $"\"{value}\"" : "null";
		}
		public static String KeyValuePair(String name, String value)
		{
			return $"{Value(name)}:{Value(value)}";
		}
		public static String KeyValuePair(String name, IEnumerable<String> values)
		{
			return values != null ? $"{Value(name)}:[{String.Join(",", values.Select(i => Value(i)))}]" : $"{Value(name)}:null";
		}

		public static String KeyValuePair(String name, Int32 value)
		{
			return $"{Value(name)}:{value}";
		}

		public static String Object(params String[] keyValuepairs)
		{
			return $"{{{String.Join(",", keyValuepairs)}}}";
		}
	}
}
