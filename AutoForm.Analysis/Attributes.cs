using AutoForm.Attributes;

using RhoMicro.CodeAnalysis;
using RhoMicro.CodeAnalysis.Attributes;

namespace AutoForm.Analysis
{
	internal static class Attributes
	{
		internal static class Factories
		{
			public static readonly IAttributeFactory<ModelPropertyAttribute> ModelProperty
				= AttributeFactory<ModelPropertyAttribute>.Create();
			public static readonly IAttributeFactory<DefaultControlAttribute> DefaultControl
				= AttributeFactory<DefaultControlAttribute>.Create();
			public static readonly IAttributeFactory<DefaultTemplateAttribute> DefaultTemplate
				= AttributeFactory<DefaultTemplateAttribute>.Create();
			public static readonly IAttributeFactory<SubModelAttribute> SubModel
				= AttributeFactory<SubModelAttribute>.Create();
		}
		public static readonly Namespace Namespace = Namespace.Create<ModelPropertyAttribute>();
		public static readonly TypeIdentifier ModelProperty = TypeIdentifier.Create<ModelPropertyAttribute>();
		public static readonly TypeIdentifier DefaultControl = TypeIdentifier.Create<DefaultControlAttribute>();
		public static readonly TypeIdentifier DefaultTemplate = TypeIdentifier.Create<DefaultTemplateAttribute>();
		public static readonly TypeIdentifier SubModel = TypeIdentifier.Create<SubModelAttribute>();
	}
}
