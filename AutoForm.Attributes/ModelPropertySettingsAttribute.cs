using System;
using System.Collections.Generic;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target as a model property and marks the containing type as a model.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public sealed class ModelPropertyAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="order">
		/// Defines the order in which the control for this property is to be rendered
		/// </param>
		/// <param name="controlType">
		/// Defines the control to be used for the subcontrol of the specific target property as <c>ControlType</c>.
		/// This overrides controls defined by <see cref="UseControlAttribute"/> on the target property type, 
		/// as well as controls defined by types annotated with <see cref="FallbackControlAttribute"/> that specify the target property type.
		/// </param>
		/// <param name="templateType">
		/// Defines the template to be used for the subcontrol of the specific target property as <c>TemplateType</c>.
		/// This overrides templates defined by <see cref="UseTemplateAttribute"/> on the target property type, 
		/// as well as controls defined by types annotated with <see cref="FallbackTemplateAttribute"/> that specify the target property type.
		/// </param>
		public ModelPropertyAttribute(Int32 order = 0, Type controlType = null, Type templateType = null)
		{
			Order = order;
		}

		public Int32 Order { get; }

		private readonly IDictionary<String, Object> _typeProperties = new Dictionary<String, Object>();
		public void SetTypeParameter(String parameterName, Object type)
		{
			if (_typeProperties.ContainsKey(parameterName))
			{
				_typeProperties[parameterName] = type;
			}
			else
			{
				_typeProperties.Add(parameterName, type);
			}
		}
		public Object GetTypeParameter(String parameterName)
		{
			var type = _typeProperties.TryGetValue(parameterName, out var value) ? value : null;

			return type;
		}
	}
}