using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Json.Analysis.Decorators
{
	internal readonly struct TypeIdentifierJsonDecorator : IJsonDecorator<TypeIdentifier>, IEquatable<TypeIdentifierJsonDecorator>
	{
		public static readonly TypeIdentifierJsonDecorator FallbackControlAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.FallbackControlAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();
		public static readonly TypeIdentifierJsonDecorator AttributesProviderAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.AttributesProviderAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();
		public static readonly TypeIdentifierJsonDecorator TemplateAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.TemplateAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();
		public static readonly TypeIdentifierJsonDecorator ControlAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.ControlAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();
		public static readonly TypeIdentifierJsonDecorator FallbackTemplateAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.FallbackTemplateAttribute.Value,
				NamespaceJsonDecorator.Attributes.Value)
			.AsJson();

		private readonly String _json;
		public RhoMicro.CodeAnalysis.TypeIdentifier Value { get; }

		public TypeIdentifierJsonDecorator(RhoMicro.CodeAnalysis.TypeIdentifier value) : this()
		{
			Value = value;
			_json = Json.Value(value.ToString());
		}

		public override Boolean Equals(Object obj)
		{
			return obj is TypeIdentifierJsonDecorator decorator && Equals(decorator);
		}

		public Boolean Equals(TypeIdentifierJsonDecorator other)
		{
			return _json == other._json;
		}

		public override Int32 GetHashCode()
		{
			return 885466328 + EqualityComparer<String>.Default.GetHashCode(_json);
		}

		public static Boolean operator ==(TypeIdentifierJsonDecorator left, TypeIdentifierJsonDecorator right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(TypeIdentifierJsonDecorator left, TypeIdentifierJsonDecorator right)
		{
			return !(left == right);
		}
	}
}
