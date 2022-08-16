namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes  the target property to be rendered using a specific control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class AutoControlPropertyControlAttribute : Attribute
    {
        public AutoControlPropertyControlAttribute(Type controlType)
        {
            ControlType = controlType;
        }
        public Type ControlType { get; }
    }
}