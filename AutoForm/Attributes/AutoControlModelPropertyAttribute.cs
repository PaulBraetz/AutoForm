/// <summary>
/// Denotes  the target property to be rendered using a specific control.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class AutoControlModelPropertyAttribute : Attribute
{
	public AutoControlModelPropertyAttribute(Type controlType)
	{
		ControlType = controlType;
	}
	public Type ControlType { get; set; }
}
