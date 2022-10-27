using AutoForm.Attributes;
using TestApp.AttributesProviders;
using TestApp.Templates;

namespace TestApp.Models
{
	public class MyModel
	{
		[ModelProperty]
		public byte Age { get; set; }

		[ModelProperty]
		public string? Name { get; set; }

		[AttributesProvider]
		public MyAttributesProvider AttributesProvider { get; } = new MyAttributesProvider();
	}
}
