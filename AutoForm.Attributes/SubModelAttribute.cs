using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target type as a submodel, causing generated controls to inherit subcontrols generated for the parent.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class SubModelAttribute:Attribute
	{
		public SubModelAttribute(Type baseModel)
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
