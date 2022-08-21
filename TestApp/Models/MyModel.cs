using TestApp.AttributesProviders;
using TestApp.Templates;

namespace TestApp.Models
{
	[AutoForm.Attributes.Model]
	public class MyModel
	{
		public Byte Age { get; set; }
		
		[AutoForm.Attributes.UseTemplate(typeof(TestApp.Templates.MyTemplate))]
		public String? Name { get; set; }

		[AutoForm.Attributes.AttributesProvider]
		public MyAttributesProvider AttributesProvider { get; } = new MyAttributesProvider();
	}
}
