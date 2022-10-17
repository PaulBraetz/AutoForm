using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Analysis
{
	internal static class Attributes
	{
		public static readonly Namespace AttrbutesNamespace
			= RhoMicro.CodeAnalysis.Namespace.Create()
			.Append("AutoForm")
			.Append("Attributes");

		public static readonly TypeIdentifier FallbackControlAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.FallbackControlAttribute.Value,
				AttrbutesNamespace);
		public static readonly TypeIdentifier AttributesProviderAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.AttributesProviderAttribute.Value,
				AttributesNamespace);
		public static readonly TypeIdentifier TemplateAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.TemplateAttribute.Value,
				AttributesNamespace);
		public static readonly TypeIdentifier ControlAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.ControlAttribute.Value,
				AttributesNamespace);
		public static readonly TypeIdentifier FallbackTemplateAttribute
			= TypeIdentifier.Create(
				TypeIdentifierNameJsonDecorator.FallbackTemplateAttribute.Value,
				AttributesNamespace;
	}
}
