using AutoForm.Attributes;

namespace TestApp.Templates.SubModel;

[DefaultTemplate(typeof(Models.SubModel), nameof(Models.SubModel.Name))]
public sealed class NameTemplate : MyTemplate<String>
{
	public override IEnumerable<KeyValuePair<String, Object>>? Attributes {
		get; set;
	}
		= new Dictionary<String, Object>()
		{
				{"label", nameof(Models.SubModel.Name) }
		};
}
