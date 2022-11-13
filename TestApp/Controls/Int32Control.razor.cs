using AutoForm.Attributes;
using AutoForm.Blazor.Controls.Abstractions;
using TestApp.Models;

namespace TestApp.Controls
{
	[DefaultControl(typeof(Int32))]
	public partial class Int32Control: OptimizedControlBase<Int32>
	{
	}
}
