using System;

using RhoMicro.CodeAnalysis;

namespace AutoForm.Blazor.Analysis.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct KeyValueTypesPairTemplate
		{
			private KeyValueTypesPairTemplate(ITypeIdentifier keyType, ITypeIdentifier valueType)
			{
				ModelType = keyType;
				MappedType = valueType;
			}

			public readonly ITypeIdentifier ModelType;
			public readonly ITypeIdentifier MappedType;

			private const String TEMPLATE = "			{typeof(" + MODEL_TYPE + "), typeof(" + CONTROL_TYPE + ")}";

			public KeyValueTypesPairTemplate WithKeyType(ITypeIdentifier modelType) => new KeyValueTypesPairTemplate(modelType, MappedType);

			public KeyValueTypesPairTemplate WithValueType(ITypeIdentifier controlType) => new KeyValueTypesPairTemplate(ModelType, controlType);

			public String Build()
			{
				return TEMPLATE
					.Replace(MODEL_TYPE, ModelType.ToString())
					.Replace(CONTROL_TYPE, MappedType.ToString());
			}

			public override String ToString() => Build();
		}
	}
}