using System;

namespace AutoForm.Attributes
{
    /// <summary>
    /// Denotes the target to be excluded from generated controls.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class ExcludeAttribute : Attribute
    {

    }
}