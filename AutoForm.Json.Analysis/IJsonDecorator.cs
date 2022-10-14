using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Json.Analysis
{
	internal interface IJson
	{
		String Json { get; }
	}
	internal interface IJsonDecorator<T> : IJson
	{
		T Value { get; }
	}
}
