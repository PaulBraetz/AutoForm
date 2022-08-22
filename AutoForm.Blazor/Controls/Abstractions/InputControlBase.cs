using Fort;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls.Abstractions
{
    public abstract class InputControlBase<TModel> : PrimitiveControlBase<TModel>
    {
        protected InputControlBase(String type, String updatesAttributeName = "value") : base("input", updatesAttributeName)
        {
            type.ThrowIfDefault(nameof(type));

            _additionalAttributes = new(new Dictionary<String, Object>()
            {
                {"type", type }
            });
        }

        private readonly ReadOnlyDictionary<String, Object> _additionalAttributes;

        protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
        {
            return Union(base.GetAdditionalAttributes(), _additionalAttributes);
        }
    }
}
