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
		public static Property Create(PropertyIdentifier name, ITypeIdentifier type, int order)
		{
			return new Property(name, type, default, default, order);
		}
		public Property WithControl(ITypeIdentifier control)
		{
			return new Property(Name, Type, control, Template, Order);
		}
		public Property WithTemplate(ITypeIdentifier template)
		{
			return new Property(Name, Type, Control, template, Order);
		}
	}
}
