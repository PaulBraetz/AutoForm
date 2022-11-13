using AutoForm.Attributes;

namespace TestLib.Models
{
	public interface IExternalModel
	{
		[ModelProperty]
		String? Name { get; set; }
	}
}