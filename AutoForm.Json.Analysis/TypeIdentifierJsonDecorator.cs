using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Json.Analysis
{
	internal readonly struct TypeIdentifierJsonDecorator : IJsonDecorator<TypeIdentifier>, IEquatable<IJson>
	{
		public static readonly IJsonDecorator<TypeIdentifier> FallbackControlAttribute
			= AutoForm.Analysis.Attributes.FallbackControlAttribute
			.AsJson();
		public static readonly IJsonDecorator<TypeIdentifier> AttributesProviderAttribute
			= AutoForm.Analysis.Attributes.AttributesProviderAttribute
			.AsJson();
		public static readonly IJsonDecorator<TypeIdentifier> TemplateAttribute
			= AutoForm.Analysis.Attributes.TemplateAttribute
			.AsJson();
		public static readonly IJsonDecorator<TypeIdentifier> ControlAttribute
			= AutoForm.Analysis.Attributes.ControlAttribute
			.AsJson();
		public static readonly IJsonDecorator<TypeIdentifier> FallbackTemplateAttribute
			= AutoForm.Analysis.Attributes.FallbackControlAttribute
			.AsJson();

		public TypeIdentifier Value { get; }

		public TypeIdentifierJsonDecorator(TypeIdentifier value) : this()
		{
			Value = value;
			_json = 
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
		public static bool operator ==(TypeIdentifierJsonDecorator left, TypeIdentifierJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(TypeIdentifierJsonDecorator left, TypeIdentifierJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
