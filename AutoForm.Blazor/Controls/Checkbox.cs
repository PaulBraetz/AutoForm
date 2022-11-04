using AutoForm.Blazor.Attributes;
using AutoForm.Blazor.Controls.Abstractions;
using Microsoft.AspNetCore.Components;

namespace AutoForm.Blazor.Controls
{
	public class Checkbox : InputControlBase<Boolean>
	{
		public Checkbox() : base("checkbox", "checked") { }

		protected override void OnParametersSet()
		{
			_additionalAttributes = new AttributeCollection("checked", BindConverter.FormatValue(Value));
			base.OnParametersSet();
		}

		private AttributeCollection _additionalAttributes;
		protected override AttributeCollection GetAdditionalAttributes()
		{
			return AttributeCollection.Union(_additionalAttributes, base.GetAdditionalAttributes());
		}
	}
}
