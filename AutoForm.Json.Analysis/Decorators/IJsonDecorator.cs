using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Json.Analysis.Decorators
{
	internal interface IJsonDecorator<T>
	{
		T Value {get;}
	}
}
