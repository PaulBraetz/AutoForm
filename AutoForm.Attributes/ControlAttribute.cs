using System;

namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes the target to be rendered using <c>ControlType</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
    public sealed class ControlAttribute : Attribute
    {
        public ControlAttribute(Type controlType)
        {
            ControlType = controlType;
        }

        public Type ControlType { get; }
    }
}
