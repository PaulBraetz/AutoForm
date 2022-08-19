using AutoForm.Attributes;

namespace TestApp.Templates
{
	public partial class PrimitiveTemplate<T>
	{
	}

	[FallbackTemplate(typeof(String))]
	public class StringTemplateMargin1 : PrimitiveTemplate<String>
	{
		protected override Byte Margin { get; } = 1;
	}
	[FallbackTemplate(typeof(Int16))]
	public class Int16TemplateMargin1 : PrimitiveTemplate<Int16>
	{
		protected override Byte Margin { get; } = 1;
	}
}
