using RhoMicro.CodeAnalysis;

namespace AutoForm.Analysis
{
	internal readonly struct Property
	{
		public readonly int Order;
		public readonly ITypeIdentifier Control;
		public readonly ITypeIdentifier Template;
		public readonly PropertyIdentifier Name;
		public readonly ITypeIdentifier Type;

		private Property(PropertyIdentifier name, ITypeIdentifier type, ITypeIdentifier control, ITypeIdentifier template, int order)
		{
			Name = name;
			Type = type;
			Control = control;
			Template = template;
			Order = order;
		}
		public static Property Create(PropertyIdentifier name, ITypeIdentifier type, ITypeIdentifier control, ITypeIdentifier template, int order)
		{
			return new Property(name, type, control, template, order);
		}
	}
}
