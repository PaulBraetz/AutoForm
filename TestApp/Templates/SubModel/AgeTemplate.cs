using AutoForm.Attributes;

namespace TestApp.Templates.SubModel;

[DefaultTemplate(typeof(Models.SubModel), nameof(Models.SubModel.Age))]
public sealed class AgeTemplate : MyTemplate<Int32>
{
	public override IEnumerable<KeyValuePair<String, Object>>? Attributes {
		get; set;
	}
		= new Dictionary<String, Object>()
		{
				{"label", nameof(Models.SubModel.Age) }
		};
}
