using System;
using System.Collections.Generic;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target type as a submodel, causing generated controls to inherit subcontrols generated for the parent.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class SubModelAttribute : Attribute
	{
		/// <param name="baseModel">
		/// Determines the base model type of the target.
		/// </param>
		/// <param name="members">
		/// Determines the members of <paramref name="baseModel"/> whose control is to be used 
		/// in controls used for corresponding members of the targeted model.
		/// </param>
		public SubModelAttribute(Type baseModel, params String[] members) => _members = members ?? Array.Empty<String>();
		/// <param name="baseModel">
		/// Determines the base model type of the target.
		/// </param>
		public SubModelAttribute(Type baseModel) : this(baseModel, Array.Empty<String>())
		{
		}

		private readonly String[] _members;
		public IEnumerable<String> Members {
			get {
				for(var i = 0; i < _members.Length; i++)
				{
					var member = _members[i];
					yield return member;
				}
			}
		}

		private readonly IDictionary<String, Object> _typeProperties = new Dictionary<String, Object>();
		public void SetTypeParameter(String parameterName, Object type)
		{
			if(_typeProperties.ContainsKey(parameterName))
			{
				_typeProperties[parameterName] = type;
			} else
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
