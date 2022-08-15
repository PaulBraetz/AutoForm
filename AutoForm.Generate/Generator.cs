using AutoForm.Generate.Blazor;
using Microsoft.CodeAnalysis;

namespace AutoForm.Generate
{
	[Generator]
	public sealed class Generator : GeneratorBase
	{
		protected override IControlsSourceGenerator GetControlGenerator()
		{
			return new BlazorSourceGenerator();
		}
	}
}
