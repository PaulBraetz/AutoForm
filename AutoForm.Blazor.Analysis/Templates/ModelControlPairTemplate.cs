using System;

namespace AutoForm.Generate.Blazor.Templates
{
    internal sealed partial class SourceFactory
    {
        private readonly struct KeyValueTypesPairTemplate
        {
            private KeyValueTypesPairTemplate(String keyType, String valueType)
            {
                ModelType = keyType;
                MappedType = valueType;
            }

            public readonly String ModelType;
            public readonly String MappedType;

            private const String TEMPLATE = "			{typeof(" + MODEL_TYPE + "), typeof(" + CONTROL_TYPE + ")}";

            public KeyValueTypesPairTemplate WithKeyType(String modelType)
            {
                return new KeyValueTypesPairTemplate(modelType, MappedType);
            }

            public KeyValueTypesPairTemplate WithValueType(String controlType)
            {
                return new KeyValueTypesPairTemplate(ModelType, controlType);
            }

            public String Build()
            {
                return TEMPLATE
                    .Replace(MODEL_TYPE, ModelType)
                    .Replace(CONTROL_TYPE, MappedType);
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}