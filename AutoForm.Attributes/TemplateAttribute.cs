using System;

namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes the target to be rendered using <c>TemplateType</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
    public sealed class TemplateAttribute : Attribute
    {
        public TemplateAttribute(Type templateType)
        {
            TemplateType = templateType;
        }

        public Type TemplateType { get; }
    }
}
