﻿using AutoForm.Attributes;

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
	[FallbackTemplate(typeof(Int32))]
	public class Int32TemplateMargin1 : PrimitiveTemplate<Int32>
	{
	}
	[FallbackTemplate(typeof(SByte))]
    public class SByteTemplateMargin1 : PrimitiveTemplate<SByte>
    {
    }
}
