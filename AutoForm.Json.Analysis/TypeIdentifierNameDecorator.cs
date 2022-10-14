using AutoForm.Attributes;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoForm.Json.Analysis
{
	internal readonly struct TypeIdentifierNameJsonDecorator : IJsonDecorator<RhoMicro.CodeAnalysis.TypeIdentifierName>, IEquatable<TypeIdentifierNameJsonDecorator>
	{
		public static readonly IJsonDecorator<TypeIdentifierName> ControlAttribute = GetAttributeName<UseControlAttribute>();
		public static readonly IJsonDecorator<TypeIdentifierName> TemplateAttribute = GetAttributeName<UseTemplateAttribute>();

		public static readonly IJsonDecorator<TypeIdentifierName> FallbackControlAttribute = GetAttributeName<FallbackControlAttribute>();
		public static readonly IJsonDecorator<TypeIdentifierName> FallbackTemplateAttribute = GetAttributeName<FallbackTemplateAttribute>();

		public static readonly IJsonDecorator<TypeIdentifierName> AttributesProviderAttribute = GetAttributeName<AttributesProviderAttribute>();

		public RhoMicro.CodeAnalysis.TypeIdentifierName Value { get; }

		public TypeIdentifierNameJsonDecorator(RhoMicro.CodeAnalysis.TypeIdentifierName value) : this()
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
		private static IJsonDecorator<TypeIdentifierName> GetAttributeName<T>()
		{
			return RhoMicro.CodeAnalysis.TypeIdentifierName.Create()
				.AppendNamePart(Regex.Replace(typeof(T).Name, @"Attribute$", string.Empty))
				.AsJson();
		}

		public static bool operator ==(TypeIdentifierNameJsonDecorator left, TypeIdentifierNameJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TypeIdentifierNameJsonDecorator left, TypeIdentifierNameJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
