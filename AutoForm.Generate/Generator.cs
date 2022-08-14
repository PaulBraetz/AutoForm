using AutoForm.Generate.Blazor;
using Microsoft.CodeAnalysis;

namespace AutoForm.Generate
{
	[Generator]
	internal class Generator : GeneratorBase
	{
		protected override IControlsSourceGenerator GetControlGenerator()
		{
			return new BlazorSourceGenerator();
		}
	}
}
