namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes the target property to be excluded from generated controls.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public sealed class AutoControlPropertyExcludeAttribute : Attribute
	{

	}
}