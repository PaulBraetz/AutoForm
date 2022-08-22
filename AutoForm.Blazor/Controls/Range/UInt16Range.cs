using AutoForm.Blazor.Attributes;
using AutoForm.Blazor.Controls.Abstractions;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls
{
    public class UInt16Range : RangeControlBase<UInt16>
    {
        private static readonly ReadOnlyDictionary<String, Object> _additionalAttributes = new(new Dictionary<String, Object>()
        {
            {"min", UInt16.MinValue.ToString() },
            {"max", UInt16.MaxValue.ToString() }
        });

        protected override AttributeCollection GetAdditionalAttributes()
        {
            return AttributeCollection.Union(_additionalAttributes, base.GetAdditionalAttributes());
        }
    }
}
