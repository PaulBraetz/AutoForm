/// <summary>
/// Denotes the target type as a control for the model type provided.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AutoControlAttribute : Attribute
{
	public AutoControlAttribute(Type modelType)
	{
		ModelType = modelType;
	}
	public Type ModelType { get; private set; }
}