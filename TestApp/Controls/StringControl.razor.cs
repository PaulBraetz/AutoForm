using AutoForm.Attributes;
using TestApp.Models;

namespace TestApp.Controls
{
	[DefaultControl(typeof(String))]
	[DefaultControl(typeof(MyModel), nameof(MyModel.Name))]
	public partial class StringControl: AutoForm.Blazor.Controls.Abstractions.ControlBase<String>
	{
	}
}
