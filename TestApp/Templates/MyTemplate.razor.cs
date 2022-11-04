using AutoForm.Attributes;
using AutoForm.Blazor.Templates.Abstractions;
using TestApp.Models;

namespace TestApp.Templates
{
	public abstract partial class MyTemplate<TModel> : TemplateBase<TModel>
	{
	}

	[DefaultTemplate(typeof(IMyModel), nameof(IMyModel.Name))]
	public sealed class StringTemplate : MyTemplate<String> { }
	[DefaultTemplate(typeof(IMyModel), nameof(IMyModel.Age))]
	public sealed class ByteTemplate : MyTemplate<Byte> { }
}
