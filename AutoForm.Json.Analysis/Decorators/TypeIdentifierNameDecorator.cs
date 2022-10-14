using AutoForm.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoForm.Json.Analysis.Decorators
{
	internal readonly struct TypeIdentifierNameJsonDecorator : IJsonDecorator<RhoMicro.CodeAnalysis.TypeIdentifierName>, IEquatable<TypeIdentifierNameJsonDecorator>
	{
		public static readonly TypeIdentifierNameJsonDecorator ControlAttribute = GetAttributeName<UseControlAttribute>();
		public static readonly TypeIdentifierNameJsonDecorator TemplateAttribute = GetAttributeName<UseTemplateAttribute>();

		public static readonly TypeIdentifierNameJsonDecorator FallbackControlAttribute = GetAttributeName<FallbackControlAttribute>();
		public static readonly TypeIdentifierNameJsonDecorator FallbackTemplateAttribute = GetAttributeName<FallbackTemplateAttribute>();

		public static readonly TypeIdentifierNameJsonDecorator AttributesProviderAttribute = GetAttributeName<AttributesProviderAttribute>();

		public RhoMicro.CodeAnalysis.TypeIdentifierName Value { get; }

		public TypeIdentifierNameJsonDecorator(RhoMicro.CodeAnalysis.TypeIdentifierName value) : this()
		{
			Value = value;
			_json = Json.Value(value.ToString());
		}

		private readonly string _json;

		public override String ToString()
		{
			return _json ?? "null";
		}

		private static TypeIdentifierNameJsonDecorator GetAttributeName<T>()
		{
			return RhoMicro.CodeAnalysis.TypeIdentifierName.Create()
				.AppendNamePart(Regex.Replace(typeof(T).Name, @"Attribute$", String.Empty))
				.AsJson();
		}

		public override Boolean Equals(Object obj)
		{
			return obj is TypeIdentifierNameJsonDecorator decorator && Equals(decorator);
		}

		public Boolean Equals(TypeIdentifierNameJsonDecorator other)
		{
			return _json == other._json;
		}

		public override Int32 GetHashCode()
		{
			return 885466328 + EqualityComparer<String>.Default.GetHashCode(_json);
		}

		public static Boolean operator ==(TypeIdentifierNameJsonDecorator left, TypeIdentifierNameJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(TypeIdentifierNameJsonDecorator left, TypeIdentifierNameJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
