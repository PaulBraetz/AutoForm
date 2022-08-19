using AutoForm.Attributes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;

namespace AutoForm.Blazor
{

    public sealed class AutoControl<TModel> : ComponentBase
    {
        private static readonly Type _controlType = Controls.ModelControlMap.TryGetValue(typeof(TModel), out Type? controlType) ?
            controlType :
            throw new Exception($"Unable to locate control for {nameof(TModel)} of {typeof(TModel).FullName}. Make sure that {typeof(TModel).FullName} is annotated with {nameof(ModelAttribute)}.");

        private static readonly Type? _templateType = Controls.ModelTemplateMap.TryGetValue(typeof(TModel), out Type? templateType) ?
            templateType :
            templateType;

        [Parameter]
        public TModel? Value { get; set; }
        [Parameter]
        public EventCallback<TModel?> ValueChanged { get; set; }
        [Parameter]
        public IDictionary<String, Object>? Attributes { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if(_templateType != null)
			{
				builder.OpenComponent(0, _templateType);
				builder.AddAttribute(1, "Value", RuntimeHelpers.TypeCheck<TModel>(Value));
				builder.AddAttribute(2, "Attributes", RuntimeHelpers.TypeCheck<IDictionary<String, Object>>(Attributes));
				builder.AddAttribute(3, "ChildContent", (RenderFragment)(buildLocation1SubControl));
				builder.CloseComponent();
			}
            else
            {
				buildLocation1SubControl(builder);
			}

			void buildLocation1SubControl(RenderTreeBuilder builder)
			{
				if(Value != null)
                {
					builder.OpenComponent(4, _controlType);
					builder.AddAttribute(5, "Value", RuntimeHelpers.TypeCheck<TModel>(Value));
					builder.AddAttribute(6, "ValueChanged", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<TModel>(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value = __value; ValueChanged.InvokeAsync(Value); }, Value))));
					builder.AddAttribute(7, "Attributes", RuntimeHelpers.TypeCheck<IDictionary<String, Object>>(Attributes));
					builder.CloseComponent();
				}
			}
		}
    }
}
