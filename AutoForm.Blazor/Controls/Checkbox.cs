using AutoForm.Blazor.Controls.Abstractions;
using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Controls
{
    public class Checkbox : InputControlBase<String>
    {
        public Checkbox() : base("checkbox", "checked") { }

        protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
        {
            return Union(base.GetAdditionalAttributes(), new[] { new KeyValuePair<String, Object>("checked", BindConverter.FormatValue(Value)) });
        }
    }
}
