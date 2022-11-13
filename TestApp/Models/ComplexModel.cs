using AutoForm.Attributes;

namespace TestApp.Models
{
	public sealed class ComplexModel
	{
		[ModelProperty]
		public String? Street { get; set; }
		[ModelProperty]
		public String? City { get; set; }
		[ModelProperty]
		public Int32 ZipCode { get; set; }
		[ModelProperty]
		public String? Address { get; set; }
		[ModelProperty]
		public MyModel NestedModel { get; set; } = new MyModel();
	}
}
