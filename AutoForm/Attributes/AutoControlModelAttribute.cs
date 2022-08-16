namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes the target type as a model for which controls should be generated if not provided.
    /// <para>
    /// The registered control for models will be available to other controls.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AutoControlModelAttribute : Attribute
    {
    }
}