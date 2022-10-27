using AutoForm.Attributes;
using AutoForm.Blazor.Templates.Abstractions;
using TestApp.Models;

namespace TestApp.Templates
{
	public abstract partial class MyTemplate<TModel> : TemplateBase<TModel>
	{
	}

	[FallbackTemplate(typeof(MyModel), nameof(MyModel.Name))]
	public sealed class StringTemplate : MyTemplate<String> { }
	[FallbackTemplate(typeof(MyModel), nameof(MyModel.Age))]
	public sealed class ByteTemplate : MyTemplate<Byte> { }
}
