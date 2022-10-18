using AutoForm.Analysis;
using Microsoft.CodeAnalysis;
using RhoMicro.CodeAnalysis;

namespace AutoForm.Json.Analysis
{
	[Generator]
	internal sealed class JsonGenerator : GeneratorBase
	{
		protected override void OnError(GeneratorExecutionContext context, Error error)
		{
			var json = JsonDecorator<Error>.Object(
				error,
				m => new IJson[]
				{
					JsonDecorator<Error>.KeyValuePair(
					nameof(Error),
					error.ToJson())
				});
			var source = $"//{json}";
			var fileName = GeneratedIdentifiers.GeneratedControls.ToString().Replace('.', '_');
			var generatedSource = new GeneratedSource(source, fileName);
			context.AddSource(generatedSource);
		}

		protected override void OnModelSpaceCreated(GeneratorExecutionContext context, ModelSpace modelSpace)
		{
			var json = JsonDecorator<ModelSpace>.Object(
				modelSpace,
				m => new IJson[]
				{
					JsonDecorator<ModelSpace>.KeyValuePair(
					nameof(ModelSpace),
					modelSpace.WithRequiredGeneratedControls(false).ToJson())
				});
			var source = $"//{json}";
			var fileName = GeneratedIdentifiers.GeneratedControls.ToString().Replace('.', '_');
			var generatedSource = new GeneratedSource(source, fileName);
			context.AddSource(generatedSource);
		}
	}
}
