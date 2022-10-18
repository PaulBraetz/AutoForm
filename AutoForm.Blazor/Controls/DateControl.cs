using AutoForm.Blazor.Controls.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace AutoForm.Blazor.Controls
{
	public class DateControl : InputControlBase<DateTime>
	{
		public DateControl() : base("date") { }
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(1, ElementName);
			builder.AddAttribute(2, "value", BindConverter.FormatValue(RootValue, "yyyy-MM-dd", CultureInfo.CurrentCulture));
			builder.AddAttribute(3, "oninput", EventCallback.Factory.CreateBinder(this, __value => RootValue = __value, RootValue, "yyyy-MM-dd", CultureInfo.CurrentCulture));
			var attributes = GetAttributes();
			if (attributes.Any())
			{
				builder.AddMultipleAttributes(4, RuntimeHelpers.TypeCheck(attributes));
			}

			builder.SetUpdatesAttributeName(UpdatesAttributeName);
			builder.CloseElement();
		}
	}
}
