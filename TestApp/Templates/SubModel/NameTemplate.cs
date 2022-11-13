using AutoForm.Attributes;

namespace TestApp.Templates.SubModel
{
	[DefaultTemplate(typeof(Models.SubModel), nameof(Models.SubModel.Name))]
	public sealed class NameTemplate : MyTemplate<string>
    {
        public override IEnumerable<KeyValuePair<string, object>>? Attributes { get; set; }
            = new Dictionary<string, object>()
            {
                {"label", nameof(Models.SubModel.Name) }
            };
    }
}
