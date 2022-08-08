using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct ControlTemplate
		{
			private ControlTemplate(String modelType, IEnumerable<SubControlTypeFieldTemplate> subControlTypeFields, IEnumerable<SubControlTemplate> subControls, ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
			{
				ModelType = modelType;
				SubControlTypeFieldTemplates = subControlTypeFields;
				SubControlTemplates = subControls;
				ControlTypeIdentifierTemplate = controlTypeIdentifierTemplate;
			}

			public readonly String ModelType;

			public readonly IEnumerable<SubControlTypeFieldTemplate> SubControlTypeFieldTemplates;
			public readonly IEnumerable<SubControlTemplate> SubControlTemplates;
			public readonly ControlTypeIdentifierTemplate ControlTypeIdentifierTemplate;

			private const String TEMPLATE =
@"		private sealed class " + CONTROL_TYPE_IDENTIFIER_TEMPLATE + @" : ComponentBase
		{
			[Parameter]
			public " + MODEL_TYPE + @"? Value { get; set; }
			[Parameter]
			public EventCallback<" + MODEL_TYPE + @"?> ValueChanged { get; set; }

" + SUB_CONTROL_TYPE_FIELDS + @"

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
				return new ControlTemplate(modelType, SubControlTypeFieldTemplates, SubControlTemplates, ControlTypeIdentifierTemplate);
			}
			public ControlTemplate WithControlTypeIdentifierTemplate(ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
			{
				return new ControlTemplate(ModelType, SubControlTypeFieldTemplates, SubControlTemplates, controlTypeIdentifierTemplate);
			}
			public ControlTemplate WithSubControlTypeFieldTemplates(IEnumerable<SubControlTypeFieldTemplate> subControlTypeFieldTemplates)
			{
				return new ControlTemplate(ModelType, subControlTypeFieldTemplates, SubControlTemplates, ControlTypeIdentifierTemplate);
			}
			public ControlTemplate WithSubControlTemplates(IEnumerable<SubControlTemplate> subControlTemplates)
			{
				return new ControlTemplate(ModelType, SubControlTypeFieldTemplates, subControlTemplates, ControlTypeIdentifierTemplate);
			}
			public String Build(ref Int32 controlIndex)
			{
				Int32 subControlTypeFieldIndex = 0;
				var subControlTypeFields = String.Join("\n", SubControlTypeFieldTemplates.Select(t => t.Build(ref subControlTypeFieldIndex)));

				subControlTypeFieldIndex = 0;
				Int32 subControlLineIndex = 0;
				var subControls = String.Join("\n", SubControlTemplates.Select(t => t.Build(ref subControlLineIndex, ref subControlTypeFieldIndex)));

				return TEMPLATE
					.Replace(CONTROL_TYPE_IDENTIFIER_TEMPLATE, ControlTypeIdentifierTemplate.Build(ref controlIndex))
					.Replace(MODEL_TYPE, ModelType)
					.Replace(SUB_CONTROL_TYPE_FIELDS, subControlTypeFields)
					.Replace(SUB_CONTROLS, subControls);
			}
		}

	}
}
