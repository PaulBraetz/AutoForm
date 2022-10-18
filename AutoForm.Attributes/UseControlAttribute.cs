using System;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Defines the control to be used for subcontrols whose model type is the target type as <c>ControlType</c>. 
	/// This overrides controls defined by types annotated with <see cref="FallbackControlAttribute"/> that specify the target type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class UseControlAttribute : Attribute
	{
		public UseControlAttribute(Type controlType)
		{
			ControlType = controlType;
		}

		public Type ControlType { get; }
	}
}
