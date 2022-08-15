using AutoForm.Generate.Blazor.Templates;
using AutoForm.Generate.Models;
using System;

namespace AutoForm.Generate.Blazor
{
	public sealed class BlazorSourceGenerator : IControlsSourceGenerator
	{
		public String Filename => "AutoFormControls_Blazor.g";

		public String Generate(ModelSpace modelSpace)
		{
			return SourceFactory.Create(modelSpace).Build();
		}

		public String Generate(Error error)
		{
			return SourceFactory.Create(error).Build();
		}
	}
}
