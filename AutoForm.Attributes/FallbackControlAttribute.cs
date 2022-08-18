using System;

namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes the target as a fallback control for <c>ModelType</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class FallbackControlAttribute : Attribute
    {
        public FallbackControlAttribute(Type modelType)
        {
            ModelType = modelType;
        }
        public Type ModelType { get; }
    }
}