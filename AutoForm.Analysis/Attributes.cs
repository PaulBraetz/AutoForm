using AutoForm.Attributes;
using RhoMicro.CodeAnalysis;
using RhoMicro.CodeAnalysis.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Analysis
{
	internal static class Attributes
	{
		internal static class Factories
		{
			public static readonly IAttributeFactory<AttributesProviderAttribute> AttributesProvider
				= AttributeFactory<AttributesProviderAttribute>.Create();
			public static readonly IAttributeFactory<ModelPropertyAttribute> ModelProperty
				= AttributeFactory<ModelPropertyAttribute>.Create();
			public static readonly IAttributeFactory<UseControlAttribute> UseControl
				= AttributeFactory<UseControlAttribute>.Create();
			public static readonly IAttributeFactory<FallbackControlAttribute> FallbackControl
				= AttributeFactory<FallbackControlAttribute>.Create();
			public static readonly IAttributeFactory<UseTemplateAttribute> UseTemplate
				= AttributeFactory<UseTemplateAttribute>.Create();
			public static readonly IAttributeFactory<FallbackTemplateAttribute> FallbackTemplate
				= AttributeFactory<FallbackTemplateAttribute>.Create();
		}
		public static readonly Namespace Namespace = Namespace.Create<ModelPropertyAttribute>();
		public static readonly TypeIdentifier AttributesProvider = TypeIdentifier.Create<AttributesProviderAttribute>();
		public static readonly TypeIdentifier ModelProperty = TypeIdentifier.Create<ModelPropertyAttribute>();
		public static readonly TypeIdentifier UseControl = TypeIdentifier.Create<UseControlAttribute>();
		public static readonly TypeIdentifier FallbackControl = TypeIdentifier.Create<FallbackControlAttribute>();
		public static readonly TypeIdentifier UseTemplate = TypeIdentifier.Create<UseTemplateAttribute>();
		public static readonly TypeIdentifier FallbackTemplate = TypeIdentifier.Create<FallbackTemplateAttribute>();
	}
}
