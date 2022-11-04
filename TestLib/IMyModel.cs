using AutoForm.Attributes;
using TestLib.AttributesProviders;

namespace TestLib.Models
{
	public interface IMyModel
	{
		[ModelProperty]
		Byte Age { get; set; }
		[AttributesProvider]
		MyAttributesProvider AttributesProvider { get; }
		[ModelProperty]
		String? Name { get; set; }

		String ToString();
	}
}