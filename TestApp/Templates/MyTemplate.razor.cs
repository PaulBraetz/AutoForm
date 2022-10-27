using AutoForm.Blazor.Templates.Abstractions;

namespace TestApp.Templates
{
	public abstract partial class MyTemplate<TModel> : TemplateBase<TModel>
	{
	}

	public sealed class StringTemplate : MyTemplate<String> { }
	public sealed class ByteTemplate : MyTemplate<Byte> { }
}
