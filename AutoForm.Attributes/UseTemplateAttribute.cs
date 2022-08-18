using System;

namespace AutoForm.Attributes
{
    /// <summary>
    /// <para>
    /// When targeting a type:
    /// </para>
    /// <para>
    /// Defines the template to be used for subcontrols whose model type is the target type as <c>TemplateType</c>. 
    /// This overrides templates defined by types annotated with <see cref="FallbackTemplateAttribute"/> that specify the target type.
    /// </para>
    /// <para>
    /// When targeting a property:
    /// </para>
    /// <para>
    /// Defines the template to be used for the subcontrol of the specific target property as <c>TemplateType</c>.
    /// This overrides templates defined by <see cref="UseTemplateAttribute"/> on the target property type, 
    /// as well as controls defined by types annotated with <see cref="FallbackTemplateAttribute"/> that specify the target property type.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
    public sealed class UseTemplateAttribute : Attribute
    {
        public UseTemplateAttribute(Type templateType)
        {
            TemplateType = templateType;
        }

        public Type TemplateType { get; }
    }
}
