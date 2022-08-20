using System;

namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target as a model for which controls should be generated if not provided.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class ModelAttribute : Attribute
	{
	}
}