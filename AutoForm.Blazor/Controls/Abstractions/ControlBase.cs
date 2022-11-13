using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Controls.Abstractions
{
	public abstract class ControlBase<TModel> : ComponentBase
	{
		[Parameter]
		public virtual TModel? Value { get; set; }

		[Parameter]
		public virtual EventCallback<TModel> ValueChanged { get; set; }

		[Parameter]
		public virtual IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }
	}
}
