using System;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target as a model property and marks the containing type as a model.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public sealed class ModelPropertyAttribute : Attribute
	{
		public ModelPropertyAttribute()
		{
		}
	}
}