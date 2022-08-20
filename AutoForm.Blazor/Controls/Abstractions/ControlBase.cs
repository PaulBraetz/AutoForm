using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Controls.Abstractions
{
	public abstract class ControlBase<TModel> : ComponentBase
	{
		[Parameter]
		public TModel? Value { get; set; }

		[Parameter]
		public EventCallback<TModel> ValueChanged { get; set; }

		[Parameter]
		public IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }
	}
}
