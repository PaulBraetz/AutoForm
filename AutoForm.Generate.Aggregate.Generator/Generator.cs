using AutoForm.Generate.Blazor;
using AutoForm.Generate.Debug;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AutoForm.Generate.Aggregate.Generator
{
	[Generator]
	public sealed class Generator : GeneratorBase
	{
		protected override IEnumerable<IControlsSourceGenerator> GetControlGenerators()
		{
			return new IControlsSourceGenerator[] { new BlazorSourceGenerator(), new DebugSourceGenerator() };
		}
	}
}
