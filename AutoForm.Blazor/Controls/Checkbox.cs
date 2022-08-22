using AutoForm.Blazor.Controls.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls
{
    public class Checkbox : InputControlBase<String>
    {
        public Checkbox() : base("checkbox", "checked") { }

        protected override void OnParametersSet()
        {
            _additionalAttributes = new(new Dictionary<String, Object>()
            {
                {"checked", BindConverter.FormatValue(Value) }
            });
            base.OnParametersSet();
        }

        private ReadOnlyDictionary<String, Object> _additionalAttributes;
        protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
        {
            return Union(base.GetAdditionalAttributes(), _additionalAttributes);
        }
    }
}
