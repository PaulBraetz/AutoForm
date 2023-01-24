using AutoForm.Attributes;

namespace TestApp.Models;

//Inherit subcontrols for all properties of MyModel
[SubModel(typeof(MyModel))]
//Inherit subcontrols for specific properties in MyModel
[SubModel(typeof(MyModel), nameof(MyModel.Name))]
public class SubModel : MyModel
{
	[ModelProperty]
	public Int32 Age {
		get; set;
	}
}
