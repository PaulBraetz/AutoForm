using AutoForm.Blazor.Attributes;
using AutoForm.Blazor.Controls.Abstractions;

namespace AutoForm.Blazor.Controls
{
	public class CharControl : InputControlBase<Char>
	{
		public CharControl() : base("text")
		{

		}
		private static readonly AttributeCollection _additionalAttributes = new("maxlength", "1")
			;
		protected override AttributeCollection GetAdditionalAttributes()
		{
			return AttributeCollection.Union(_additionalAttributes, base.GetAdditionalAttributes());
		}
	}
}
