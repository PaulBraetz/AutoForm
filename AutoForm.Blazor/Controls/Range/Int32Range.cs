using AutoForm.Blazor.Controls.Abstractions;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls
{
    public class Int32Range : RangeControlBase<Int32>
    {
        private static readonly ReadOnlyDictionary<String, Object> _attributes = new(new Dictionary<String, Object>()
        {
            {"min", Int32.MinValue.ToString() },
            {"max", Int32.MaxValue.ToString() }
        });

        protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
        {
            return Union(base.GetAdditionalAttributes(), _attributes);
        }
    }
}
