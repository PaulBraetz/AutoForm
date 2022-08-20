using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Templates
{
	public abstract class TemplateBase<TModel> : ComponentBase
	{
		[Parameter]
		public TModel? Value { get; set; }
		[Parameter]
		public IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }
		[Parameter]
		public RenderFragment? ChildContent { get; set; }
	}
}
