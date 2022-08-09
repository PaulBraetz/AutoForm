using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct ControlTemplate
		{
			private ControlTemplate(String modelType, IEnumerable<SubControlTemplate> subControls, ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
			{
				ModelType = modelType;
				SubControlTemplates = subControls;
				ControlTypeIdentifierTemplate = controlTypeIdentifierTemplate;
			}

			public readonly String ModelType;

			public readonly IEnumerable<SubControlTemplate> SubControlTemplates;
			public readonly ControlTypeIdentifierTemplate ControlTypeIdentifierTemplate;

			private const String TEMPLATE =
@"		///<summary>
		///Generated control for models of type <cref see=""" + MODEL_TYPE + @"""/>.
		///</summary>
		private sealed class " + CONTROL_TYPE_IDENTIFIER_TEMPLATE + @" : ComponentBase
		{
			[Parameter]
			public " + MODEL_TYPE + @"? Value { get; set; }
			[Parameter]
			public EventCallback<" + MODEL_TYPE + @"?> ValueChanged { get; set; }

			protected override void BuildRenderTree(RenderTreeBuilder builder)
			{		
				if(Value != null)
				{
" + SUB_CONTROLS + @"
				}
			}
		}";

			public ControlTemplate WithModelType(String modelType)
			{
				return new ControlTemplate(modelType, SubControlTemplates, ControlTypeIdentifierTemplate);
			}
			public ControlTemplate WithControlTypeIdentifierTemplate(ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
			{
				return new ControlTemplate(ModelType, SubControlTemplates, controlTypeIdentifierTemplate);
			}
			public ControlTemplate WithSubControlTemplates(IEnumerable<SubControlTemplate> subControlTemplates)
			{
				return new ControlTemplate(ModelType, subControlTemplates, ControlTypeIdentifierTemplate);
			}
			public String Build(ref Int32 controlIndex)
			{
				Int32 subControlLineIndex = 0;
				var subControls = String.Join("\n", SubControlTemplates.Select(t => t.Build(ref subControlLineIndex)));

				return TEMPLATE
					.Replace(CONTROL_TYPE_IDENTIFIER_TEMPLATE, ControlTypeIdentifierTemplate.Build(ref controlIndex))
					.Replace(MODEL_TYPE, ModelType)
					.Replace(SUB_CONTROLS, subControls);
			}

            public override String ToString()
			{
				Int32 subControlLineIndex = 0;
				var subControls = String.Join("\n", SubControlTemplates.Select(t => t.Build(ref subControlLineIndex)));

				return TEMPLATE
					.Replace(CONTROL_TYPE_IDENTIFIER_TEMPLATE, ControlTypeIdentifierTemplate.ToString())
					.Replace(MODEL_TYPE, ModelType)
					.Replace(SUB_CONTROLS, subControls);
			}
        }

	}
}
