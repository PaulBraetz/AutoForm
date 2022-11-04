using AutoForm.Attributes;
using TestApp.AttributesProviders;
using TestApp.Templates;

namespace TestApp.Models
{
	[SubModel(typeof(IMyModel))]
	public class MyModel : IMyModel
	{
		public byte Age { get; set; }

		public string? Name { get; set; }

		public MyAttributesProvider AttributesProvider { get; } = new MyAttributesProvider();

		public override String ToString()
		{
			return $"In Model! {Name}, {Age}";
		}
	}
}
