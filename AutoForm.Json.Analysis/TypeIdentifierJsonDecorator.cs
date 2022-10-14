using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Json.Analysis
{
	internal readonly struct TypeIdentifierJsonDecorator : IJsonDecorator<TypeIdentifier>, IEquatable<IJson>
	{
		public static readonly IJsonDecorator<TypeIdentifier> FallbackControlAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.FallbackControlAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();
		public static readonly IJsonDecorator<TypeIdentifier> AttributesProviderAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.AttributesProviderAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();
		public static readonly IJsonDecorator<TypeIdentifier> TemplateAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.TemplateAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();
		public static readonly IJsonDecorator<TypeIdentifier> ControlAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.ControlAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();
		public static readonly IJsonDecorator<TypeIdentifier> FallbackTemplateAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.FallbackTemplateAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();

		public TypeIdentifier Value { get; }

		public TypeIdentifierJsonDecorator(TypeIdentifier value) : this()
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
