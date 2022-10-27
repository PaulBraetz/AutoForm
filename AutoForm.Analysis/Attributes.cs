using AutoForm.Attributes;
using RhoMicro.CodeAnalysis;
using RhoMicro.CodeAnalysis.Attributes;

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
			public static readonly IAttributeFactory<ControlAttribute> Control
				= AttributeFactory<ControlAttribute>.Create();
			public static readonly IAttributeFactory<TemplateAttribute> Template
				= AttributeFactory<TemplateAttribute>.Create();
		}
		public static readonly Namespace Namespace = Namespace.Create<ModelPropertyAttribute>();
		public static readonly TypeIdentifier AttributesProvider = TypeIdentifier.Create<AttributesProviderAttribute>();
		public static readonly TypeIdentifier ModelProperty = TypeIdentifier.Create<ModelPropertyAttribute>();
		public static readonly TypeIdentifier Control = TypeIdentifier.Create<ControlAttribute>();
		public static readonly TypeIdentifier FallbackTemplate = TypeIdentifier.Create<TemplateAttribute>();
	}
}
