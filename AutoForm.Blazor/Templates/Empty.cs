using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AutoForm.Blazor.Templates
{
    public sealed class Empty : ComponentBase
    {
        [Parameter]
        public Object? Value { get; set; }
        [Parameter]
        public IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.AddContent(0, ChildContent);
        }
    }
}
