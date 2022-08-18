using System;

namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes the target as a fallback template for controls for <c>ModelType</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class FallbackTemplateAttribute : Attribute
    {
        public FallbackTemplateAttribute(Type modelType)
        {
            ModelType = modelType;
        }

        public Type ModelType { get; }
    }
}
