using AutoForm.Attributes;
using TestLib.AttributesProviders;

namespace TestLib.Models
{
	public interface IMyModel
	{
		[ModelProperty]
		Byte Age { get; set; }
		[ModelProperty]
		String? Name { get; set; }

		String ToString();
	}
}