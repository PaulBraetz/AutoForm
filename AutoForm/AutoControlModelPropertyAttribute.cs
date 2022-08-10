[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public sealed class AutoControlModelPropertyAttribute : Attribute
{
	public AutoControlModelPropertyAttribute(Type controlType)
	{
		ControlType = controlType;
	}
	public AutoControlModelPropertyAttribute()
	{

	}
	public Type? ControlType { get; set; }
}