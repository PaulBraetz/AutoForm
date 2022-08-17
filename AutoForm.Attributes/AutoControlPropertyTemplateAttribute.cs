namespace AutoForm.Attributes
{
	/// <summary>
	/// Denotes  the target property to be rendered inside of a specific template.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class AutoControlPropertyTemplateAttribute : Attribute
    {
        public AutoControlPropertyTemplateAttribute(Type templateType)
        {
            TemplateType = templateType;
        }
        public Type TemplateType { get; }
    }
}