using System;

namespace AutoForm.Attributes
{
	/// <summary>
	/// <para>
	/// When targeting a type:
	/// </para>
	/// <para>
	/// Defines the control to be used for subcontrols whose model type is the target type as <c>ControlType</c>. 
	/// This overrides controls defined by types annotated with <see cref="FallbackControlAttribute"/> that specify the target type.
	/// </para>
	/// <para>
	/// When targeting a property:
	/// </para>
	/// <para>
	/// Defines the control to be used for the subcontrol of the specific target property as <c>ControlType</c>.
	/// This overrides controls defined by <see cref="UseControlAttribute"/> on the target property type, 
	/// as well as controls defined by types annotated with <see cref="FallbackControlAttribute"/> that specify the target property type.
	/// </para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
	public sealed class UseControlAttribute : Attribute
	{
		public UseControlAttribute(Type controlType)
		{
			ControlType = controlType;
		}

		public Type ControlType { get; }
	}
}
