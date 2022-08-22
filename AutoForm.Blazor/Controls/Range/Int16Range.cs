using AutoForm.Blazor.Controls.Abstractions;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls
{
    public class Int16Range : RangeControlBase<Int16>
    {
        private static readonly ReadOnlyDictionary<String, Object> _additionalAttributes = new(new Dictionary<String, Object>()
        {
            {"min", Int16.MinValue.ToString() },
            {"max", Int16.MaxValue.ToString() }
        });

        protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
        {
            return Union(base.GetAdditionalAttributes(), _additionalAttributes);
        }
    }
}
