using System;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target as a model property and marks the containing type as a model.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public sealed class ModelPropertyAttribute : Attribute
	{
		/// <summary>
		/// Defines the order in which the control for this property is to be rendered
		/// </summary>
		public Int32 Order { get; set; }

		/// <summary>
		/// Defines the control to be used for the subcontrol of the specific target property as <c>ControlType</c>.
		/// This overrides controls defined by <see cref="UseControlAttribute"/> on the target property type, 
		/// as well as controls defined by types annotated with <see cref="FallbackControlAttribute"/> that specify the target property type.
		/// </summary>
		public Type ControlType { get; set; }

		/// <summary>
		/// Defines the template to be used for the subcontrol of the specific target property as <c>TemplateType</c>.
		/// This overrides templates defined by <see cref="UseTemplateAttribute"/> on the target property type, 
		/// as well as controls defined by types annotated with <see cref="FallbackTemplateAttribute"/> that specify the target property type.
		/// </summary>
		public Type TemplateType { get; set; }
	}
}