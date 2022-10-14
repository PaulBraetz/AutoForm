using System;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Defines the template to be used for subcontrols whose model type is the target type as <c>TemplateType</c>. 
	/// This overrides templates defined by types annotated with <see cref="FallbackTemplateAttribute"/> that specify the target type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class UseTemplateAttribute : Attribute
	{
		public UseTemplateAttribute(Type templateType)
		{
			TemplateType = templateType;
		}

		public Type TemplateType { get; }
	}
}
