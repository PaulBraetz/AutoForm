using TestApp.Models;

namespace TestApp.Controls
{
	[AutoForm.Attributes.DefaultControl(typeof(String))]
	[AutoForm.Attributes.DefaultControl(typeof(MyModel), nameof(MyModel.Name))]
	public partial class StringControl: AutoForm.Blazor.Controls.Abstractions.ControlBase<String>
	{
	}
}
