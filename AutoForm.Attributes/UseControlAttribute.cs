using System;
using System.Collections.Generic;

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
		}

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
