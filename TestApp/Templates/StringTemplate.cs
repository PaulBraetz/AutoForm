using AutoForm.Attributes;
using TestApp.Models;

namespace TestApp.Templates
{
	[DefaultTemplate(typeof(String))]
	[DefaultTemplate(typeof(MyModel), nameof(MyModel.Name))]
	public sealed class StringTemplate : MyTemplate<String> { }
}
