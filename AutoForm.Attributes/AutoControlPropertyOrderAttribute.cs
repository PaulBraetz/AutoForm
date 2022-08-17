namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes in what order the control is to be rendered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class AutoControlPropertyOrderAttribute : Attribute
    {
        public AutoControlPropertyOrderAttribute(Int32 order)
        {
            Order = order;
        }
        public Int32 Order { get; }
    }
}