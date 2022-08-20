using Fort;

namespace AutoForm.Blazor.Controls.Abstractions
{
	public abstract class InputControlBase<TModel> : PrimitiveControlBase<TModel>
	{
		protected InputControlBase(String type, String updatesAttributeName = "value") : base("input", updatesAttributeName)
		{
			type.ThrowIfDefault(nameof(type));

			_type = type;
		}

		private readonly String _type;

		protected override IEnumerable<KeyValuePair<String, Object>>? GetAdditionalAttributes()
		{
			return Union(base.GetAdditionalAttributes(), new[] { new KeyValuePair<String, Object>("type", _type) });
		}
	}
}
