using AutoForm.Analysis.Models;
using System;
using System.Text.RegularExpressions;

namespace AutoForm.Generate.Blazor.Templates
{
    internal sealed partial class SourceFactory
    {
        private readonly struct SubControlTemplate
        {
            private SubControlTemplate(String modelType,
                                       String propertyIdentifier,
                                       String propertyType,
                                       String controlType,
                                       String templateType,
                                       SubControlPassAttributesTemplate subControlPassAttributesTemplate)
            {
                _modelType = modelType;
                _propertyIdentifier = propertyIdentifier;
                _propertyType = propertyType;
                _controlType = controlType;
                _templateType = templateType;
                _subControlPassAttributesTemplate = subControlPassAttributesTemplate;
            }

            private readonly String _modelType;
            private readonly String _propertyIdentifier;
            private readonly String _propertyType;
            private readonly String _controlType;
            private readonly String _templateType;

            private readonly SubControlPassAttributesTemplate _subControlPassAttributesTemplate;

            private const String TEMPLATE =
@"                    //Subcontrol for " + PROPERTY_IDENTIFIER + @"
                    __builder.OpenComponent<" + CONTROL_TYPE + @">(" + LINE_INDEX + @");
                    __builder.AddAttribute(" + LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value." + PROPERTY_IDENTIFIER + @"));
                    __builder.AddAttribute(" + LINE_INDEX + @", ""ValueChanged"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::Microsoft.AspNetCore.Components.EventCallback<" + PROPERTY_TYPE + @">>(global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<" + PROPERTY_TYPE + @">(this, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value." + PROPERTY_IDENTIFIER + @" = __value; ValueChanged.InvokeAsync(Value);}, Value." + PROPERTY_IDENTIFIER + @"))));" + SUB_CONTROL_PASS_ATTRIBUTES + @"
                    __builder.CloseComponent();";

            private const String TEMPLATED_TEMPLATE =
@"					//Template for " + PROPERTY_IDENTIFIER + @"
                    __builder.OpenComponent<" + TEMPLATE_TYPE + @">(" + LINE_INDEX + @");
                    __builder.AddAttribute(" + LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value));" + SUB_CONTROL_PASS_ATTRIBUTES + @"
                    __builder.AddAttribute(" + LINE_INDEX + @", ""ChildContent"", (Microsoft.AspNetCore.Components.RenderFragment)(build" + PROPERTY_IDENTIFIER + @"SubControl));
                    __builder.CloseComponent();
					
                    void build" + PROPERTY_IDENTIFIER + @"SubControl(RenderTreeBuilder __builder)
					{
" + TEMPLATE + @"
					}";
            public SubControlTemplate WithModelType(String modelType)
            {
                return new SubControlTemplate(modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _controlType,
                                              _templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithPropertyIdentifier(String propertyIdentifier)
            {
                return new SubControlTemplate(_modelType,
                                              propertyIdentifier,
                                              _propertyIdentifier,
                                              _controlType,
                                              _templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithPropertyType(String propertyType)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              propertyType,
                                              _controlType,
                                              _templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithControlType(String controlType)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              controlType,
                                              _templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithTemplateType(String templateType)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _controlType,
                                              templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithSubControlPassAttributesTemplate(SubControlPassAttributesTemplate attributesProviderIdentifierTemplate)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _controlType,
                                              _templateType,
                                              attributesProviderIdentifierTemplate);
            }

            public String Build()
            {
                var subControlPassAttributes = _subControlPassAttributesTemplate.Build();

                var template = (String.IsNullOrEmpty(_templateType) ?
                                TEMPLATE :
                                TEMPLATED_TEMPLATE
                                    .Replace(TEMPLATE_TYPE, _templateType))
                                    .Replace(SUB_CONTROL_PASS_ATTRIBUTES, subControlPassAttributes)
                                    .Replace(MODEL_TYPE, _modelType)
                                    .Replace(PROPERTY_IDENTIFIER, _propertyIdentifier)
                                    .Replace(PROPERTY_TYPE, _propertyType)
                                    .Replace(CONTROL_TYPE, _controlType);

                return template;
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}