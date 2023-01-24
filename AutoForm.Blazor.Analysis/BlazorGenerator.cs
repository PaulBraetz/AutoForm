using AutoForm.Analysis;
using AutoForm.Blazor.Analysis.Templates;

using Microsoft.CodeAnalysis;

using RhoMicro.CodeAnalysis;

namespace AutoForm.Blazor.Analysis
{
	[Generator]
	internal sealed class BlazorGenerator : GeneratorBase
	{
		private const System.String FILE_NAME = "AutoForm_Blazor";

		protected override void OnError(GeneratorExecutionContext context, Error error) => AddSource(context, SourceFactory.Create(error));

		protected override void OnModelSpaceCreated(GeneratorExecutionContext context, ModelSpace modelSpace) => AddSource(context, SourceFactory.Create(modelSpace));

		private void AddSource(GeneratorExecutionContext context, SourceFactory factory)
		{
			var source = new GeneratedSource(factory.Build(), FILE_NAME);
			context.AddSource(source);
		}
	}
}
