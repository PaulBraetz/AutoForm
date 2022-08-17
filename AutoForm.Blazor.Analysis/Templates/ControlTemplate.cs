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
                           IEnumerable<SubControlTemplate> subControlTemplates,
                           ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
            {
                _modelType = modelType;
                _subControlTemplates = subControlTemplates;
                _controlTypeIdentifierTemplate = controlTypeIdentifierTemplate;
            }

            private readonly String _modelType;

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
                    if(Attributes?.Any() ?? false)
				    {
                        __builder.OpenElement(0, ""div"");
				        __builder.AddMultipleAttributes(1, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<global::System.String, global::System.Object>>>(Attributes));
                    }

" + SUB_CONTROLS + @"

                    if(Attributes != null)
				    {
                        __builder.CloseElement();
                    }
				}
			}
            #pragma warning restore 1998
		}";

            public ControlTemplate WithModelType(String modelType)
            {
                return new ControlTemplate(modelType, _subControlTemplates, _controlTypeIdentifierTemplate);
            }
            public ControlTemplate WithControlTypeIdentifierTemplate(ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
            {
                return new ControlTemplate(_modelType, _subControlTemplates, controlTypeIdentifierTemplate);
            }
            public ControlTemplate WithSubControlTemplates(IEnumerable<SubControlTemplate> subControlTemplates)
            {
                return new ControlTemplate(_modelType, subControlTemplates, _controlTypeIdentifierTemplate);
            }
            public String Build()
            {
                Int32 subControlLineIndex = 2;

                var subControls = String.Join("\n\n", _subControlTemplates.Select(t => t.Build()));
                
                var regex = new Regex(Regex.Escape(SUB_CONTROL_LINE_INDEX));
                while (regex.IsMatch(subControls))
                {
                    subControls = regex.Replace(subControls, subControlLineIndex.ToString(), 1);
                    subControlLineIndex++;
                }

                var result = TEMPLATE
                    .Replace(MODEL_TYPE, _modelType)
                    .Replace(SUB_CONTROLS, subControls)
                    .Replace(CONTROL_TYPE_IDENTIFIER, _controlTypeIdentifierTemplate.Build());

                return result;
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}