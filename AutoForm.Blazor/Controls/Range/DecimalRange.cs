using AutoForm.Blazor.Controls.Abstractions;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls
{
    public class DecimalRange : RangeControlBase<Decimal>
    {
        private static readonly ReadOnlyDictionary<String, Object> _additionalAttributes = new(new Dictionary<String, Object>()
        {
            {"min", Decimal.MinValue.ToString() },
            {"max", Decimal.MaxValue.ToString() }
        });

        protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
        {
            return Union(base.GetAdditionalAttributes(), _additionalAttributes);
        }
    }
}
