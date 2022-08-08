using Fort;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AutoControlAttribute : Attribute
{
	public AutoControlAttribute(Type modelType)
	{
		modelType.ThrowIfDefault(nameof(modelType));
		ModelType = modelType;
	}
	public Type ModelType { get; private set; }
}
