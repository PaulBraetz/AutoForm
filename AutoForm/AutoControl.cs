using AutoForm.Attributes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;

namespace AutoForm
{

    public sealed class AutoControl<TModel> : ComponentBase
    {
        private static readonly Type _controlType = Controls.ModelControlMap.TryGetValue(typeof(TModel), out Type? controlType) ?
            controlType :
            throw new Exception($"Unable to locate control for {nameof(TModel)} of {typeof(TModel).FullName}. Make sure that {typeof(TModel).FullName}is annotated with {nameof(AutoControlModelAttribute)}.");

        [Parameter]
        public TModel? Value { get; set; }
        [Parameter]
        public EventCallback<TModel?> ValueChanged { get; set; }
        [Parameter]
        public IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent(0, _controlType);
            builder.AddAttribute(1, "Value", RuntimeHelpers.TypeCheck(Value));
            builder.AddAttribute(2, "ValueChanged", RuntimeHelpers.TypeCheck(ValueChanged));
            builder.AddAttribute(3, "Attributes", RuntimeHelpers.TypeCheck(Attributes));
            builder.CloseComponent();
        }
    }
}
