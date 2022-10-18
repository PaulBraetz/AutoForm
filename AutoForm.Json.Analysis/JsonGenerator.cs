using AutoForm.Analysis;
using Microsoft.CodeAnalysis;
using RhoMicro.CodeAnalysis;

namespace AutoForm.Json.Analysis
{
	[Generator]
	internal sealed class JsonGenerator : GeneratorBase
	{
		private const System.String FILE_NAME = "AutoForm_Json";

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
			AddSource(context, json);
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
			AddSource(context, json);
		}

		private void AddSource(GeneratorExecutionContext context, IJson json)
		{
			var source = new GeneratedSource($"//{json.Json}", FILE_NAME);
			context.AddSource(source);
		}
	}
}
