using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Templates.Abstractions
{
    public abstract class TemplateBase<TModel> : ComponentBase
    {
        [Parameter]
        public TModel? Value { get; set; }
        [Parameter]
        public IEnumerable<KeyValuePair<string, object>>? Attributes { get; set; }
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}
