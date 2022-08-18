using AutoForm.Analysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoForm.Generate.Blazor.Templates
{
    internal sealed partial class SourceFactory
    {
        private readonly struct ControlTemplate
        {
            private ControlTemplate(String modelType,
                                    String templateType,
                                    IEnumerable<SubControlTemplate> subControlTemplates,
                                    ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
            {
                _modelType = modelType;
                _templateType = templateType;
                _subControlTemplates = subControlTemplates;
                _controlTypeIdentifierTemplate = controlTypeIdentifierTemplate;
            }

            private readonly String _modelType;
            private readonly String _templateType;

            private readonly IEnumerable<SubControlTemplate> _subControlTemplates;
            private readonly ControlTypeIdentifierTemplate _controlTypeIdentifierTemplate;

            private const String TEMPLATE =
@"		///<summary>
		///Generated control for models of type <cref see=""" + MODEL_TYPE + @"""/>.
		///</summary>
		private sealed class " + CONTROL_TYPE_IDENTIFIER + @" : __ControlBase<" + MODEL_TYPE + @">
		{
			#pragma warning disable 1998
			protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
			{	
				if(Value != null)
				{
" + SUB_CONTROLS + @"
				}
			}
            #pragma warning restore 1998
		}";

            private const String TEMPLATED_TEMPLATE =
@"		///<summary>
		///Generated control for models of type <cref see=""" + MODEL_TYPE + @"""/>.
		///</summary>
		private sealed class " + CONTROL_TYPE_IDENTIFIER + @" : __ControlBase<" + MODEL_TYPE + @">
		{
			#pragma warning disable 1998
			protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
			{	
				if(Value != null)
				{
                    //Template for " + MODEL_TYPE + @"
                    __builder.OpenComponent<" + TEMPLATE_TYPE + @">(" + LINE_INDEX + @");
                    __builder.AddAttribute(" + LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + MODEL_TYPE + @">(Value));
                    __builder.AddAttribute(" + LINE_INDEX + @", ""Attributes"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<global::System.String, global::System.Object>>>(Attributes));
                    __builder.AddAttribute(" + LINE_INDEX + @", ""ChildContent"", (Microsoft.AspNetCore.Components.RenderFragment)(BuildSubControls));
                    __builder.CloseComponent();
				}
			}

            private void BuildSubControls(RendertreeBuilder __builder)
            {
" + SUB_CONTROLS + @"
            }
            #pragma warning restore 1998
		}";

            public ControlTemplate WithModelType(String modelType)
            {
                return new ControlTemplate(modelType, _templateType, _subControlTemplates, _controlTypeIdentifierTemplate);
            }
            public ControlTemplate WithTemplateType(String templateType)
            {
                return new ControlTemplate(_modelType, templateType, _subControlTemplates, _controlTypeIdentifierTemplate);
            }
            public ControlTemplate WithControlTypeIdentifierTemplate(ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
            {
                return new ControlTemplate(_modelType, _templateType, _subControlTemplates, controlTypeIdentifierTemplate);
            }
            public ControlTemplate WithSubControlTemplates(IEnumerable<SubControlTemplate> subControlTemplates)
            {
                return new ControlTemplate(_modelType, _templateType, subControlTemplates, _controlTypeIdentifierTemplate);
            }
            public String Build()
            {
                Int32 lineIndex = 0;

                var subControls = String.Join("\n\n", _subControlTemplates.Select(t => t.Build()));

                var template = (String.IsNullOrEmpty(_templateType) ?
                                TEMPLATE :
                                TEMPLATED_TEMPLATE
                                    .Replace(TEMPLATE_TYPE, _templateType))
                                    .Replace(MODEL_TYPE, _modelType)
                                    .Replace(SUB_CONTROLS, subControls)
                                    .Replace(CONTROL_TYPE_IDENTIFIER, _controlTypeIdentifierTemplate.Build());

                var regex = new Regex(Regex.Escape(LINE_INDEX));
                while (regex.IsMatch(template))
                {
                    template = regex.Replace(template, lineIndex.ToString(), 1);
                    lineIndex++;
                }

                return template;
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}