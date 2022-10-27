using AutoForm.Attributes;
using TestApp.AttributesProviders;
using TestApp.Templates;

namespace TestApp.Models
{
	public class MyModel
	{
		[ModelProperty(templateType: typeof(ByteTemplate))]
		public byte Age { get; set; }

		[ModelProperty(templateType: typeof(StringTemplate))]
		public string? Name { get; set; }

		[AttributesProvider]
		public MyAttributesProvider AttributesProvider { get; } = new MyAttributesProvider();
	}
}
