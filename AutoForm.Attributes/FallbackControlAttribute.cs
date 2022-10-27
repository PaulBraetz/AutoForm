using System;
using System.Collections.Generic;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target as a fallback control for subcontrols whose model type is <c>ModelType</c>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class FallbackControlAttribute : Attribute
	{
		public FallbackControlAttribute(Type modelType)
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