using AutoForm.Attributes;

namespace TestApp.Models;

public class MyModel
{
	[ModelProperty]
	public String? Name {
		get;
		set;
	}
}
