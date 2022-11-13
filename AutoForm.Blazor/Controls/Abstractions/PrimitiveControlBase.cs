using AutoForm.Blazor.Attributes;
using Fort;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace AutoForm.Blazor.Controls.Abstractions
{
	public abstract class PrimitiveControlBase<TModel> : OptimizedControlBase<TModel>
	{
		protected TModel? RootValue
		{
			get => Value;
			set
			{
				Value = value;
				_ = ValueChanged.InvokeAsync(Value);
			}
		}

		protected readonly String ElementName;
		protected readonly String UpdatesAttributeName;

		protected PrimitiveControlBase(String elementName, String updatesAttributeName = "value")
		{
			elementName.ThrowIfDefault(nameof(elementName));
			updatesAttributeName.ThrowIfDefault(nameof(updatesAttributeName));

			ElementName = elementName;
			UpdatesAttributeName = updatesAttributeName;
		}

		protected virtual AttributeCollection GetAdditionalAttributes()
		{
			return AttributeCollection.Empty;
		}

		protected AttributeCollection GetAttributes()
		{
			return AttributeCollection.Union(Attributes, GetAdditionalAttributes());
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(1, ElementName);
			builder.AddAttribute(2, "value", BindConverter.FormatValue(RootValue, CultureInfo.CurrentCulture));
			builder.AddAttribute(3, "oninput", EventCallback.Factory.CreateBinder(this, __value => RootValue = __value, RootValue, CultureInfo.CurrentCulture));
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
