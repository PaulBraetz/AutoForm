using AutoForm.Blazor.Controls.Abstractions;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls
{
	public class UInt64Range : RangeControlBase<UInt64>
	{
		private static readonly ReadOnlyDictionary<String, Object> _attributes = new ReadOnlyDictionary<string, object>(new Dictionary<String, Object>()
		{
			{"min", UInt64.MinValue.ToString() },
			{"max", UInt64.MaxValue.ToString() }
		});

		protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
		{
			return Union(base.GetAdditionalAttributes(), _attributes);
		}
	}
}
