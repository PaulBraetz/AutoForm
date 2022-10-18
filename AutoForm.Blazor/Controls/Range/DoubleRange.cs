using AutoForm.Blazor.Attributes;
using AutoForm.Blazor.Controls.Abstractions;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls
{
	public class DoubleRange : RangeControlBase<Double>
	{
		private static readonly ReadOnlyDictionary<String, Object> _additionalAttributes = new(new Dictionary<String, Object>()
		{
			{"min", Double.MinValue.ToString() },
			{"max", Double.MaxValue.ToString() }
		});

		protected override AttributeCollection GetAdditionalAttributes()
		{
			return AttributeCollection.Union(_additionalAttributes, base.GetAdditionalAttributes());
		}
	}
}
