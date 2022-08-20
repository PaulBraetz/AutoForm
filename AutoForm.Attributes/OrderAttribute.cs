using System;

namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes in what order the control is to be rendered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class OrderAttribute : Attribute
    {
        public OrderAttribute(Int32 order)
        {
            Order = order;
        }
        public Int32 Order { get; }
    }
}