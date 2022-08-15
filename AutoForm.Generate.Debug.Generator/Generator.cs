using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AutoForm.Generate.Debug
{
	[Generator]
	public sealed class Generator : GeneratorBase
	{
		protected override IEnumerable<IControlsSourceGenerator> GetControlGenerators()
		{
			return new[] { new DebugSourceGenerator() };
		}
	}
}
