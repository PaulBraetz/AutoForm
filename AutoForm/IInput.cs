using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AutoForm
{
	public interface IInput<T> : IComponent, IHandleEvent, IHandleAfterRender
	{
		[Parameter]
		public T Value { get; set; }
		[Parameter]
		public EventCallback<T> ValueChanged { get; set; }
	}
}
