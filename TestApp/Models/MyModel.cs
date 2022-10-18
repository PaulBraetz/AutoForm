using AutoForm.Attributes;
using TestApp.AttributesProviders;
using TestApp.Templates;

namespace TestApp.Models
{
	[AutoForm.Attributes.Model]
	public class MyModel
	{
		[UseTemplate(typeof(ByteTemplate))]
		public byte Age { get; set; }

		[UseTemplate(typeof(StringTemplate))]
		public string? Name { get; set; }

		[AttributesProvider]
		public MyAttributesProvider AttributesProvider { get; } = new MyAttributesProvider();
	}
}
