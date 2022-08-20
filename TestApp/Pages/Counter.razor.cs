using AutoForm.Attributes;

namespace TestApp.Pages
{
	public partial class Counter
	{
		private void IncrementCount()
		{
			_model.Value += 1;
		}

		private Model _model = new();

		[AutoForm.Attributes.Model]
		public sealed class Model
		{
			public Int32 Value { get; set; }

			[AttributesProvider]
			public AttributesProvider Attributes { get; } = new();
		}

		public sealed class AttributesProvider
		{
			public IEnumerable<KeyValuePair<String, Object>> GetValueAttributes() => new Dictionary<String, Object>()
			{
				{  "class", "form-control" }
			};
		}
	}
}
