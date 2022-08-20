using Fort;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace AutoForm.Blazor.Controls.Abstractions
{
	public abstract class PrimitiveControlBase<TModel> : ControlBase<TModel>
	{
		private sealed class KeyValuePairComparer : IEqualityComparer<KeyValuePair<String, Object>>
		{
			private KeyValuePairComparer() { }

			public static readonly KeyValuePairComparer Instance = new();

			public Boolean Equals(KeyValuePair<String, Object> x, KeyValuePair<String, Object> y)
			{
				return x.Key == y.Key;
			}

			public Int32 GetHashCode([DisallowNull] KeyValuePair<String, Object> obj)
			{
				return obj.Key.GetHashCode();
			}
		}

		private TModel? RootValue
		{
			get => Value;
			set
			{
				Value = value;
				ValueChanged.InvokeAsync(Value);
			}
		}

		private readonly string _elementName;
		private readonly String _updatesAttributeName;

		private static readonly IEnumerable<KeyValuePair<String, Object>> _emptyAttributes = Array.Empty<KeyValuePair<String, Object>>();

		protected PrimitiveControlBase(String elementName, String updatesAttributeName = "value")
		{
			elementName.ThrowIfDefault(nameof(elementName));
			updatesAttributeName.ThrowIfDefault(nameof(updatesAttributeName));

			_elementName = elementName;
			_updatesAttributeName = updatesAttributeName;
		}

		protected static IEnumerable<KeyValuePair<String, Object>> Union(IEnumerable<KeyValuePair<String, Object>>? first, IEnumerable<KeyValuePair<String, Object>>? second)
		{
			return first == null ? second ?? _emptyAttributes :
				second == null ? first : first.Union(second);
		}

		protected virtual IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
		{
			return null;
		}

		private IEnumerable<KeyValuePair<String, Object>> GetAttributes()
		{
			return Union(Attributes, GetAdditionalAttributes());
		}

		protected sealed override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(1, _elementName);
			builder.AddAttribute(2, "value", BindConverter.FormatValue(RootValue));
			builder.AddAttribute(3, "oninput", EventCallback.Factory.CreateBinder(this, __value => RootValue = __value, RootValue));
			var attributes = GetAttributes();
			if (attributes.Any())
			{
				builder.AddMultipleAttributes(4, RuntimeHelpers.TypeCheck(attributes));
			}

			builder.SetUpdatesAttributeName(_updatesAttributeName);
			builder.CloseElement();
		}
	}
}
