using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Templates.Abstractions
{
	public abstract class TemplateBase<TModel> : ComponentBase
	{
		[Parameter]
		public virtual TModel? Value { get; set; }
		[Parameter]
		public virtual IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }
		[Parameter]
		public virtual RenderFragment? ChildContent { get; set; }
	}
}
