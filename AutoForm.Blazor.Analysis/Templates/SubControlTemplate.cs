using AutoForm.Analysis.Models;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AutoForm.Generate.Blazor.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct SubControlTemplate
		{
			private SubControlTemplate(Model model,
									   Property property,
									   SubControlPassAttributesTemplate subControlPassAttributesTemplate)
			{
				_model = model;
				_property = property;
				_subControlPassAttributesTemplate = subControlPassAttributesTemplate;
			}

			private readonly Model _model;
			private readonly Property _property;

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
			public SubControlTemplate WithModel(Model model)
			{
				return new SubControlTemplate(model,
											  _property,
											  _subControlPassAttributesTemplate);
			}
			public SubControlTemplate WithProperty(Property property)
			{
				return new SubControlTemplate(_model,
											  property,
											  _subControlPassAttributesTemplate);
			}
			public SubControlTemplate WithSubControlPassAttributesTemplate(SubControlPassAttributesTemplate subControlPassAttributesTemplate)
			{
				return new SubControlTemplate(_model,
											  _property,
											  subControlPassAttributesTemplate);
			}

			public String Build()
			{
				var subControlPassAttributes = _subControlPassAttributesTemplate.Build();

				var template = (_property.Template == default ?
								TEMPLATE :
								TEMPLATED_TEMPLATE
									.Replace(TEMPLATE_TYPE, _property.Template.ToEscapedString()))
									.Replace(SUB_CONTROL_PASS_ATTRIBUTES, subControlPassAttributes)
									.Replace(MODEL_TYPE, _model.Name.ToEscapedString())
									.Replace(PROPERTY_IDENTIFIER, _property.Name.ToEscapedString())
									.Replace(PROPERTY_TYPE, _property.Type.ToEscapedString())
									.Replace(CONTROL_TYPE, _property.Control.ToEscapedString());

				return template;
			}

			public override String ToString()
			{
				return Build();
			}
		}
	}
}