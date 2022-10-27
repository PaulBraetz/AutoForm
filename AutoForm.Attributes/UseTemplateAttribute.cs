using System;
using System.Collections.Generic;

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
