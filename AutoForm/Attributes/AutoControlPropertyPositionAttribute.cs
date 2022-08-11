/// <summary>
/// Denotes  the target property to be rendered using a specific control.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited =false)]
public sealed class AutoControlPropertyPositionAttribute : Attribute
{
	public AutoControlPropertyPositionAttribute(Int32 position)
	{
		Position = position;
	}
	public Int32 Position { get; set; }
}