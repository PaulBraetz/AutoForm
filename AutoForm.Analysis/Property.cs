using RhoMicro.CodeAnalysis;

namespace AutoForm.Analysis
{
	internal readonly struct Property
	{
		public readonly ITypeIdentifier Control;
		public readonly ITypeIdentifier Template;
		public readonly PropertyIdentifier Name;
		public readonly ITypeIdentifier Type;

		private Property(PropertyIdentifier name, ITypeIdentifier type, ITypeIdentifier control, ITypeIdentifier template)
		{
			Name = name;
			Type = type;
			Control = control;
			Template = template;
		}
		public static Property Create(PropertyIdentifier name, ITypeIdentifier type) => new Property(name, type, default, default);
		public Property WithControl(ITypeIdentifier control) => new Property(Name, Type, control, Template);
		public Property WithTemplate(ITypeIdentifier template) => new Property(Name, Type, Control, template);
	}
}
