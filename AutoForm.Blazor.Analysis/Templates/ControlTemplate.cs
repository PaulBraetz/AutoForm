using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoForm.Blazor.Analysis.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct ControlTemplate
		{
			private ControlTemplate(TypeIdentifier modelType,
									SubControlTemplate[] subControlTemplates,
									TypeIdentifier controlType)
			{
				_modelType = modelType;
				_subControlTemplates = subControlTemplates ?? Array.Empty<SubControlTemplate>();
				_controlType = controlType;
			}

			private readonly TypeIdentifier _modelType;
			private readonly TypeIdentifier _controlType;

			private readonly SubControlTemplate[] _subControlTemplates;

			private const String TEMPLATE =
@"		///<summary>
		///Generated control for models of type <cref see=""" + MODEL_TYPE + @"""/>.
		///</summary>
		private sealed class " + CONTROL_TYPE_IDENTIFIER + @" : AutoForm.Blazor.Controls.Abstractions.ControlBase<" + MODEL_TYPE + @">
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

			public ControlTemplate WithModelType(TypeIdentifier modelType)
			{
				return new ControlTemplate(modelType, _subControlTemplates, _controlType);
			}
			public ControlTemplate WithControlType(TypeIdentifier controlType)
			{
				return new ControlTemplate(_modelType, _subControlTemplates, controlType);
			}
			public ControlTemplate WithSubControlTemplates(IEnumerable<SubControlTemplate> subControlTemplates)
			{
				return new ControlTemplate(_modelType, subControlTemplates.ToArray(), _controlType);
			}
			public String Build()
			{
				var lineIndex = 0;

				var subControls = String.Join("\n\n", _subControlTemplates.Select(t => t.Build()));

				var template = TEMPLATE
					.Replace(MODEL_TYPE, _modelType.ToEscapedString())
					.Replace(SUB_CONTROLS, subControls)
					.Replace(CONTROL_TYPE_IDENTIFIER, _controlType.ToEscapedString());

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