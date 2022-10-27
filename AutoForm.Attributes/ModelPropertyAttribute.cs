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
		/// <param name="order">
		/// Defines the order in which the control for this property is to be rendered
		/// </param>
		public ModelPropertyAttribute(Int32 order = 0)
		{
			Order = order;
		}

		public Int32 Order { get; }
	}
}