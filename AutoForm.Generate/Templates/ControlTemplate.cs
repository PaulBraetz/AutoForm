using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate
{
    internal static partial class Source
    {
        private readonly struct ControlTemplate
        {
            private ControlTemplate(String modelType,
                           IEnumerable<SubControlTemplate> subControlTemplates,
                           ControlTypeIdentifierTemplate controlTypeIdentifierTemplate,
                           IEnumerable<SubControlPropertyTemplate> subControlPropertyTemplates)
            {
                _modelType = modelType;
                _subControlTemplates = subControlTemplates;
                _controlTypeIdentifierTemplate = controlTypeIdentifierTemplate;
                _subControlPropertyTemplates = subControlPropertyTemplates;
            }

            private readonly String _modelType;

            private readonly IEnumerable<SubControlPropertyTemplate> _subControlPropertyTemplates;
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
                    if(Attributes != null)
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

" + SUB_CONTROL_PROPERTIES + @"
		}";

            public ControlTemplate WithModelType(String modelType)
            {
                return new ControlTemplate(modelType, _subControlTemplates, _controlTypeIdentifierTemplate, _subControlPropertyTemplates);
            }
            public ControlTemplate WithControlTypeIdentifierTemplate(ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
            {
                return new ControlTemplate(_modelType, _subControlTemplates, controlTypeIdentifierTemplate, _subControlPropertyTemplates);
            }
            public ControlTemplate WithSubControlTemplates(IEnumerable<SubControlTemplate> subControlTemplates)
            {
                return new ControlTemplate(_modelType, subControlTemplates, _controlTypeIdentifierTemplate, _subControlPropertyTemplates);
            }
            public ControlTemplate WithSubControlPropertyTemplates(IEnumerable<SubControlPropertyTemplate> subControlPropertyTemplates)
            {
                return new ControlTemplate(_modelType, _subControlTemplates, _controlTypeIdentifierTemplate, subControlPropertyTemplates);
            }

            public String Build()
            {
                Int32 subControlLineIndex = 2;
                var subControls = String.Join("\n\n", _subControlTemplates.Select(t => t.Build(ref subControlLineIndex)));

                var subControlProperties = String.Empty;//String.Join("\n\n", _subControlPropertyTemplates.Select(t => t.Build()));

                var result = TEMPLATE
                    .Replace(MODEL_TYPE, _modelType)
                    .Replace(SUB_CONTROL_PROPERTIES, subControlProperties)
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
