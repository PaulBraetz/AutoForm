using AutoForm.Generate;
using AutoForm.Generate.Models;
using System;

namespace ConsoleGenerator
{
	internal readonly struct ConsoleSourceGenerator : IControlsSourceGenerator
	{
		public String Generate(ModelSpace modelSpace)
		{
			return modelSpace.ToString();
		}

		public String Generate(Error error)
		{
			return error.ToString();
		}
	}
}
