using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;

namespace AutoForm.Blazor
{
    public sealed class TextArea : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "textarea");
            builder.AddAttribute(1, "value", BindConverter.FormatValue(__Value));
            builder.AddAttribute(2, "oninput", EventCallback.Factory.CreateBinder(this, __value => __Value = __value, __Value));
            if (Attributes != null)
            {
                builder.AddMultipleAttributes(3, RuntimeHelpers.TypeCheck(Attributes));
            }
            builder.SetUpdatesAttributeName("value");
            builder.CloseElement();
        }

        [Parameter]
        public String? Value { get; set; }
        [Parameter]
        public EventCallback<String?> ValueChanged { get; set; }
        [Parameter]
        public IDictionary<String, Object>? Attributes { get; set; }

        private global::System.String? __Value
        {
            get => Value;
            set
            {
                Value = value;
                ValueChanged.InvokeAsync(Value);
            }
        }
    }
}
