using System;
using System.Text.RegularExpressions;
using static AutoForm.Generate.Blazor.BlazorSourceGenerator;

namespace AutoForm.Generate.Blazor.Templates
{
    internal readonly partial struct SourceFactory
    {
        private readonly struct SubControlTemplate
        {
            private SubControlTemplate(String modelType,
                                       String propertyIdentifier,
                                       String propertyType,
                                       String subControlType,
                                       SubControlPassAttributesTemplate subControlPassAttributesTemplate,
                                       SubControlPropertyIdentifierTemplate subControlPropertyIdentifierTemplate)
            {
                _modelType = modelType;
                _propertyIdentifier = propertyIdentifier;
                _propertyType = propertyType;
                _subControlType = subControlType;
                _subControlPassAttributesTemplate = subControlPassAttributesTemplate;
                _subControlPropertyIdentifierTemplate = subControlPropertyIdentifierTemplate;
            }

            private readonly String _modelType;
            private readonly String _propertyIdentifier;
            private readonly String _propertyType;
            private readonly String _subControlType;

            private readonly SubControlPassAttributesTemplate _subControlPassAttributesTemplate;
            private readonly SubControlPropertyIdentifierTemplate _subControlPropertyIdentifierTemplate;

            private const String TEMPLATE =
    @"                    //SubControl for " + PROPERTY_IDENTIFIER + @"
                    __builder.OpenComponent<" + SUB_CONTROL_TYPE + @">(" + SUB_CONTROL_LINE_INDEX + @");
                    __builder.AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value." + PROPERTY_IDENTIFIER + @"));
                    __builder.AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""ValueChanged"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::Microsoft.AspNetCore.Components.EventCallback<" + PROPERTY_TYPE + @">>(global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<" + PROPERTY_TYPE + @">(this, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value." + PROPERTY_IDENTIFIER + @" = __value; ValueChanged.InvokeAsync(Value);}, Value." + PROPERTY_IDENTIFIER + @"))));" + SUB_CONTROL_PASS_ATTRIBUTES + @"
                    __builder.CloseComponent();";

            public SubControlTemplate WithModelType(String modelType)
            {
                return new SubControlTemplate(modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _subControlType,
                                              _subControlPassAttributesTemplate,
                                              _subControlPropertyIdentifierTemplate);
            }
            public SubControlTemplate WithPropertyIdentifier(String propertyIdentifier)
            {
                return new SubControlTemplate(_modelType,
                                              propertyIdentifier,
                                              _propertyIdentifier,
                                              _subControlType,
                                              _subControlPassAttributesTemplate,
                                              _subControlPropertyIdentifierTemplate);
            }
            public SubControlTemplate WithPropertyType(String propertyType)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              propertyType,
                                              _subControlType,
                                              _subControlPassAttributesTemplate,
                                              _subControlPropertyIdentifierTemplate);
            }
            public SubControlTemplate WithSubControlType(String subControlType)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              subControlType,
                                              _subControlPassAttributesTemplate,
                                              _subControlPropertyIdentifierTemplate);
            }
            public SubControlTemplate WithSubControlPassAttributesTemplate(SubControlPassAttributesTemplate attributesProviderIdentifierTemplate)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _subControlType,
                                              attributesProviderIdentifierTemplate,
                                              _subControlPropertyIdentifierTemplate);
            }
            public SubControlTemplate WithSubControlPropertyIdentifierTemplate(SubControlPropertyIdentifierTemplate subControlPropertyIdentifierTemplate)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _subControlType,
                                              _subControlPassAttributesTemplate,
                                              subControlPropertyIdentifierTemplate);
            }

            public String Build(ref Int32 subControlLineIndex)
            {
                var subControlPassAttributes = _subControlPassAttributesTemplate.Build();
                var subControlPropertyIdentifier = _subControlPropertyIdentifierTemplate.Build();

                var template = TEMPLATE
                    .Replace(SUB_CONTROL_PASS_ATTRIBUTES, subControlPassAttributes)
                    .Replace(MODEL_TYPE, _modelType)
                    .Replace(PROPERTY_IDENTIFIER, _propertyIdentifier)
                    .Replace(PROPERTY_TYPE, _propertyType)
                    .Replace(SUB_CONTROL_TYPE, _subControlType)
                    .Replace(SUB_CONTROL_PROPERTY_IDENTIFIER, subControlPropertyIdentifier);

                var regex = new Regex(Regex.Escape(SUB_CONTROL_LINE_INDEX));
                while (regex.IsMatch(template))
                {
                    template = regex.Replace(template, subControlLineIndex.ToString(), 1);
                    subControlLineIndex++;
                }

                return template;
            }

            public override String ToString()
            {
                Int32 subControlIndex = 0;
                return Build(ref subControlIndex);
            }
        }
    }
}