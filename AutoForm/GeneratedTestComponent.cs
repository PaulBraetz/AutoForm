﻿// <auto-generated/>
#pragma warning disable 1591
namespace AutoForm.Generated
{
	using System;
	using Microsoft.AspNetCore.Components;
	public partial class GeneratedTestComponent : Microsoft.AspNetCore.Components.ComponentBase, IInput<String?>
	{
#pragma warning disable 1998
		protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
		{
			__builder.OpenElement(0, "input");
			__builder.AddAttribute(1, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(Value));
			__builder.AddAttribute(2, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Value = __value, Value));
			__builder.SetUpdatesAttributeName("value");
			__builder.CloseElement();
		}
#pragma warning restore 1998
		[Parameter]
		public String Value { get; set; }
		[Parameter]
		public EventCallback<String> ValueChanged { get; set; }
	}
}
#pragma warning restore 1591