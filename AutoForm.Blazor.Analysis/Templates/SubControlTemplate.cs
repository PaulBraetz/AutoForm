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
                                       String subControlType,
                                       String builderName,
                                       SubControlPassAttributesTemplate subControlPassAttributesTemplate)
            {
                _modelType = modelType;
                _propertyIdentifier = propertyIdentifier;
                _propertyType = propertyType;
                _subControlType = subControlType;
                _templateType = builderName;
                _subControlPassAttributesTemplate = subControlPassAttributesTemplate;
            }

            private readonly String _modelType;
            private readonly String _propertyIdentifier;
            private readonly String _propertyType;
            private readonly String _subControlType;
            private readonly String _templateType;

            private readonly SubControlPassAttributesTemplate _subControlPassAttributesTemplate;

            private const String TEMPLATE =
@"                    //Subcontrol for " + PROPERTY_IDENTIFIER + @"
                    " + BUILDER_NAME + @".OpenComponent<" + SUB_CONTROL_TYPE + @">(" + SUB_CONTROL_LINE_INDEX + @");
                    " + BUILDER_NAME + @".AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value." + PROPERTY_IDENTIFIER + @"));
                    " + BUILDER_NAME + @".AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""ValueChanged"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::Microsoft.AspNetCore.Components.EventCallback<" + PROPERTY_TYPE + @">>(global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<" + PROPERTY_TYPE + @">(this, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value." + PROPERTY_IDENTIFIER + @" = __value; ValueChanged.InvokeAsync(Value);}, Value." + PROPERTY_IDENTIFIER + @"))));" + SUB_CONTROL_PASS_ATTRIBUTES + @"
                    " + BUILDER_NAME + @".CloseComponent();";

            private const String TEMPLATED_TEMPLATE =
@"                    //Templated subcontrol for " + PROPERTY_IDENTIFIER + @"
                    " + TEMPLATE_BUILDER_NAME + @".OpenComponent<" + TEMPLATE_TYPE + @">(" + SUB_CONTROL_LINE_INDEX + @");
                    " + TEMPLATE_BUILDER_NAME + @".AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value." + PROPERTY_IDENTIFIER + @"));
                    " + TEMPLATE_BUILDER_NAME + @".AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""ChildContent"", (Microsoft.AspNetCore.Components.RenderFragment)((" + BUILDER_NAME + @") => {
" + TEMPLATE + @"
                    }
                    ));
                    " + TEMPLATE_BUILDER_NAME + @".CloseComponent();";

            public SubControlTemplate WithModelType(String modelType)
            {
                return new SubControlTemplate(modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _subControlType,
                                              _templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithPropertyIdentifier(String propertyIdentifier)
            {
                return new SubControlTemplate(_modelType,
                                              propertyIdentifier,
                                              _propertyIdentifier,
                                              _subControlType,
                                              _templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithPropertyType(String propertyType)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              propertyType,
                                              _subControlType,
                                              _templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithSubControlType(String subControlType)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              subControlType,
                                              _templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithTemplateType(String templateType)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _subControlType,
                                              templateType,
                                              _subControlPassAttributesTemplate);
            }
            public SubControlTemplate WithSubControlPassAttributesTemplate(SubControlPassAttributesTemplate attributesProviderIdentifierTemplate)
            {
                return new SubControlTemplate(_modelType,
                                              _propertyIdentifier,
                                              _propertyType,
                                              _subControlType,
                                              _templateType,
                                              attributesProviderIdentifierTemplate);
            }

            public String Build()
            {
                var subControlPassAttributes = _subControlPassAttributesTemplate.Build();

                var template = (String.IsNullOrEmpty(_templateType) ?
                                TEMPLATE
                                    .Replace(SUB_CONTROL_PASS_ATTRIBUTES, subControlPassAttributes)
                                    .Replace(BUILDER_NAME, "__builder") :
                                TEMPLATED_TEMPLATE
                                    .Replace(SUB_CONTROL_PASS_ATTRIBUTES, subControlPassAttributes)
                                    .Replace(TEMPLATE_BUILDER_NAME, "__builder")
                                    .Replace(BUILDER_NAME, "__builder_2")
                                    .Replace(TEMPLATE_TYPE, _templateType))
                                    .Replace(MODEL_TYPE, _modelType)
                                    .Replace(PROPERTY_IDENTIFIER, _propertyIdentifier)
                                    .Replace(PROPERTY_TYPE, _propertyType)
                                    .Replace(SUB_CONTROL_TYPE, _subControlType);

                return template;
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}