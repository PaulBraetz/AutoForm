﻿using System;

using AutoForm.Analysis;

namespace AutoForm.Blazor.Analysis.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct SubControlTemplate
		{
			private SubControlTemplate(Model model,
									   Property property)
			{
				_model = model;
				_property = property;
			}

			private readonly Model _model;
			private readonly Property _property;

			private const String TEMPLATE =
@"					//Subcontrol for " + PROPERTY_IDENTIFIER + @"
                    __builder.OpenComponent<" + CONTROL_TYPE + @">(" + LINE_INDEX + @");
                    __builder.AddAttribute(" + LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value." + PROPERTY_IDENTIFIER + @"));
                    __builder.AddAttribute(" + LINE_INDEX + @", ""ValueChanged"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::Microsoft.AspNetCore.Components.EventCallback<" + PROPERTY_TYPE + @">>(global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<" + PROPERTY_TYPE + @">(this, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => { Value." + PROPERTY_IDENTIFIER + @" = __value; ValueChanged.InvokeAsync(Value);}, Value." + PROPERTY_IDENTIFIER + @"))));
                    __builder.CloseComponent();";

			private const String TEMPLATED_TEMPLATE =
@"					//Template for " + PROPERTY_IDENTIFIER + @"
                    __builder.OpenComponent<" + TEMPLATE_TYPE + @">(" + LINE_INDEX + @");
                    __builder.AddAttribute(" + LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value." + PROPERTY_IDENTIFIER + @"));
                    __builder.AddAttribute(" + LINE_INDEX + @", ""ChildContent"", (Microsoft.AspNetCore.Components.RenderFragment)(build" + PROPERTY_IDENTIFIER + @"SubControl));
                    __builder.CloseComponent();
					
                    void build" + PROPERTY_IDENTIFIER + @"SubControl(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
					{
" + TEMPLATE + @"
					}";
			public SubControlTemplate WithModel(Model model)
			{
				return new SubControlTemplate(model,
											  _property);
			}
			public SubControlTemplate WithProperty(Property property)
			{
				return new SubControlTemplate(_model,
											  property);
			}
			public String Build()
			{
				var template = (_property.Template == default ?
								TEMPLATE :
								TEMPLATED_TEMPLATE
									.Replace(TEMPLATE_TYPE, _property.Template.ToString()))
									.Replace(MODEL_TYPE, _model.Name.ToString())
									.Replace(PROPERTY_IDENTIFIER, _property.Name.ToString())
									.Replace(PROPERTY_TYPE, _property.Type.ToString())
									.Replace(CONTROL_TYPE, _property.Control.ToString());

				return template;
			}

			public override String ToString() => Build();
		}
	}
}