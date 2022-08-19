using AutoForm.Attributes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;

namespace AutoForm.Blazor
{

	public sealed class AutoControl<TModel> : ComponentBase
	{
		private static readonly Type _controlType = GeneratedControls.ModelControlMap?.TryGetValue(typeof(TModel), out Type? controlType) ?? false ?
			controlType :
			throw new Exception($"Unable to locate control for {nameof(TModel)} of {typeof(TModel).FullName}. Make sure that {typeof(TModel).FullName} is annotated with {nameof(ModelAttribute)}.");

		private static readonly Type? _templateType = GeneratedControls.ModelTemplateMap?.TryGetValue(typeof(TModel), out Type? templateType) ?? false ?
			templateType :
			null;

		[Parameter]
		public TModel? Value { get; set; }
		[Parameter]
		public EventCallback<TModel?> ValueChanged { get; set; }
		[Parameter]
		public IDictionary<String, Object>? Attributes { get; set; }

		private Action<RenderTreeBuilder>? _renderStrategy;
		private Action<RenderTreeBuilder> RenderStrategy=> _renderStrategy ??= _templateType == null ?
																				RenderSubControl:
																				RenderTemplatedSubControl;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			RenderStrategy.Invoke(builder);
		}

		private void RenderTemplatedSubControl(RenderTreeBuilder builder)
		{
			builder.OpenComponent(0, _templateType!);
			builder.AddAttribute(1, "Value", RuntimeHelpers.TypeCheck<TModel>(Value));
			builder.AddAttribute(2, "Attributes", RuntimeHelpers.TypeCheck<IDictionary<String, Object>?>(Attributes));
			builder.AddAttribute(3, "ChildContent", (RenderFragment)(RenderSubControl));
			builder.CloseComponent();
		}
		private void RenderSubControl(RenderTreeBuilder builder)
		{
			if (Value != null)
			{
				builder.OpenComponent(4, _controlType);
				builder.AddAttribute(5, "Value", RuntimeHelpers.TypeCheck<TModel>(Value));
				builder.AddAttribute(6, "ValueChanged", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create<TModel>(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value = __value; ValueChanged.InvokeAsync(Value); }, Value))));
				builder.AddAttribute(7, "Attributes", RuntimeHelpers.TypeCheck<IDictionary<String, Object>?>(Attributes));
				builder.CloseComponent();
			}
		}
	}
}
