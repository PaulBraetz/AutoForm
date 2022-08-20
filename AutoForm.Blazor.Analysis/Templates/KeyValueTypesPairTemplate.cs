using AutoForm.Analysis.Models;
using System;

namespace AutoForm.Blazor.Analysis.Templates
{
    internal sealed partial class SourceFactory
    {
        private readonly struct KeyValueTypesPairTemplate
        {
            private KeyValueTypesPairTemplate(TypeIdentifier keyType, TypeIdentifier valueType)
            {
                ModelType = keyType;
                MappedType = valueType;
            }

            public readonly TypeIdentifier ModelType;
            public readonly TypeIdentifier MappedType;

            private const String TEMPLATE = "			{typeof(" + MODEL_TYPE + "), typeof(" + CONTROL_TYPE + ")}";

            public KeyValueTypesPairTemplate WithKeyType(TypeIdentifier modelType)
            {
                return new KeyValueTypesPairTemplate(modelType, MappedType);
            }

            public KeyValueTypesPairTemplate WithValueType(TypeIdentifier controlType)
            {
                return new KeyValueTypesPairTemplate(ModelType, controlType);
            }

            public String Build()
            {
                return TEMPLATE
                    .Replace(MODEL_TYPE, ModelType.ToEscapedString())
                    .Replace(CONTROL_TYPE, MappedType.ToEscapedString());
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}