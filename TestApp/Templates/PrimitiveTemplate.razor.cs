using AutoForm.Attributes;

namespace TestApp.Templates
{
	public partial class PrimitiveTemplate<T>
	{
	}

	[FallbackTemplate(typeof(String))]
	public class StringTemplateMargin1 : PrimitiveTemplate<String>
	{
	}
	[FallbackTemplate(typeof(Int16))]
	public class Int16TemplateMargin1 : PrimitiveTemplate<Int16>
	{
	}
}
