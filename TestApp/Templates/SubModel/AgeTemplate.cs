using AutoForm.Attributes;

namespace TestApp.Templates.SubModel
{
    [DefaultTemplate(typeof(Models.SubModel), nameof(Models.SubModel.Age))]
    public sealed class AgeTemplate : MyTemplate<int>
    {
        public override IEnumerable<KeyValuePair<string, object>>? Attributes { get; set; }
            = new Dictionary<string, object>()
            {
                {"label", nameof(Models.SubModel.Age) }
            };
    }
}
