using System;
using System.Collections.Generic;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target as a fallback template for subcontrols whose model type is <c>modelType</c> or subcontrols of <c>members</c> of <c>modelType</c>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public sealed class TemplateAttribute : Attribute
	{
		/// <param name="modelType">
		/// When <paramref name="members"/> is not set, determines the model for which the targeted template is to be used.<br/>
		/// When <paramref name="members"/> is set, determines the model whose members are to be rendered using the targeted template.
		/// </param>
		/// <param name="members">
		/// Determines the members of <paramref name="modelType"/> that are to be rendered using the targeted template.
		/// </param>
		public TemplateAttribute(Type modelType, params String[] members)
		{
			_members = members ?? Array.Empty<String>();
		}

		private readonly String[] _members;
		public IEnumerable<String> Members
		{
			get
			{
				for (var i = 0; i < _members.Length; i++)
				{
					var member = _members[i];
					yield return member;
				}
			}
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
