using Fort;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AutoControlAttribute : Attribute
{
	public AutoControlAttribute(Type modelType)
	{
		ModelType = modelType;
	}
	public Type ModelType { get; private set; }
}
