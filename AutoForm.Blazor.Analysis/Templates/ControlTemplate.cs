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
			private ControlTemplate(Model model,
									IEnumerable<SubControlTemplate> subControlTemplates,
									ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
			{
				_model = model;
				_subControlTemplates = subControlTemplates;
				_controlTypeIdentifierTemplate = controlTypeIdentifierTemplate;
			}

			private readonly Model _model;

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

			public ControlTemplate WithModel(Model model)
			{
				return new ControlTemplate(model, _subControlTemplates, _controlTypeIdentifierTemplate);
			}
			public ControlTemplate WithControlTypeIdentifierTemplate(ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
			{
				return new ControlTemplate(_model, _subControlTemplates, controlTypeIdentifierTemplate);
			}
			public ControlTemplate WithSubControlTemplates(IEnumerable<SubControlTemplate> subControlTemplates)
			{
				return new ControlTemplate(_model, subControlTemplates, _controlTypeIdentifierTemplate);
			}
			public String Build()
			{
				Int32 lineIndex = 0;

				var subControls = String.Join("\n\n", _subControlTemplates.Select(t => t.Build()));

				var template = TEMPLATE
					.Replace(MODEL_TYPE, _model.Name.ToEscapedString())
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