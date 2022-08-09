using Microsoft.AspNetCore.Components;
using TestApp.Models.NestedNamespace;

namespace TestApp.Controls
{
	[AutoControl(typeof(List<MyModel>))]
	public partial class MyModelListControl
	{
		[Parameter]
		public List<MyModel>? Value { get; set; }
		[Parameter]
		public EventCallback<List<MyModel>?> ValueChanged { get; set; }

		private void ModelChanged(MyModel? model)
		{
			ValueChanged.InvokeAsync(Value);
		}
	}
}
