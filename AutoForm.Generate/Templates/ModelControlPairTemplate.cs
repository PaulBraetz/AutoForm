using System;
using static AutoForm.Generate.Blazor.BlazorSourceGenerator;

namespace AutoForm.Generate.Blazor.Templates
{
    internal readonly partial struct SourceFactory
    {
        private readonly struct ModelControlPairTemplate
        {
            private ModelControlPairTemplate(String modelType, String controlType)
            {
                ModelType = modelType;
                ControlType = controlType;
            }

            public readonly String ModelType;
            public readonly String ControlType;

            private const String TEMPLATE = "			{typeof(" + MODEL_TYPE + "), typeof(" + CONTROL_TYPE + ")}";

            public ModelControlPairTemplate WithModelType(String modelType)
            {
                return new ModelControlPairTemplate(modelType, ControlType);
            }

            public ModelControlPairTemplate WithControlType(String controlType)
            {
                return new ModelControlPairTemplate(ModelType, controlType);
            }

            public String Build()
            {
                return TEMPLATE
                    .Replace(MODEL_TYPE, ModelType)
                    .Replace(CONTROL_TYPE, ControlType);
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}