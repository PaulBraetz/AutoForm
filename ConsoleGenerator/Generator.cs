using AutoForm.Generate;
using Microsoft.CodeAnalysis;

namespace ConsoleGenerator
{
	[Generator]
	public sealed class Generator : GeneratorBase
	{
		protected override IControlsSourceGenerator GetControlGenerator()
		{
			return new ConsoleSourceGenerator();
		}
	}
}
