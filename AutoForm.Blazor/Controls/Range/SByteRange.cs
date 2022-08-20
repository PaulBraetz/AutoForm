using AutoForm.Blazor.Controls.Abstractions;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls
{
	public class SByteRange : RangeControlBase<SByte>
	{
		private static readonly ReadOnlyDictionary<String, Object> _attributes = new ReadOnlyDictionary<string, object>(new Dictionary<String, Object>()
		{
			{"min", SByte.MinValue.ToString() },
			{"max", SByte.MaxValue.ToString() }
		});

		protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
		{
			return Union(base.GetAdditionalAttributes(), _attributes);
		}
	}
}
