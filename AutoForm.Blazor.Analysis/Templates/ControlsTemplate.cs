﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Blazor.Analysis.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct ControlsTemplate
		{
			private ControlsTemplate(KeyValueTypesPairTemplate[] modelControlPairTemplates, KeyValueTypesPairTemplate[] modelTemplatePairTemplates, ControlTemplate[] controlTemplates)
			{
				_modelControlPairTemplates = modelControlPairTemplates ?? Array.Empty<KeyValueTypesPairTemplate>();
				_modelTemplatePairTemplates = modelTemplatePairTemplates ?? Array.Empty<KeyValueTypesPairTemplate>();
				_controlTemplates = controlTemplates ?? Array.Empty<ControlTemplate>();
			}

			private readonly KeyValueTypesPairTemplate[] _modelControlPairTemplates;
			private readonly KeyValueTypesPairTemplate[] _modelTemplatePairTemplates;
			private readonly ControlTemplate[] _controlTemplates;

			private const String TEMPLATE =
	@"#pragma warning disable 1591
using Microsoft.AspNetCore.Components;
namespace AutoForm.Blazor
{	
	public static class GeneratedControls
	{
#region Type Maps
		public static readonly IReadOnlyDictionary<Type, Type> ModelControlMap = new global::System.Collections.ObjectModel.ReadOnlyDictionary<Type, Type>(new Dictionary<Type, Type>()
		{
" + MODEL_CONTROL_PAIRS + @"
		});

		public static readonly IReadOnlyDictionary<Type, Type> ModelTemplateMap = new global::System.Collections.ObjectModel.ReadOnlyDictionary<Type, Type>(new Dictionary<Type, Type>()
		{
" + MODEL_TEMPLATE_PAIRS + @"
		});
#endregion

#region Generated Controls
" + CONTROLS + @"
#endregion
	}
}
#pragma warning restore 1591";

			public ControlsTemplate WithModelControlPairTemplates(IEnumerable<KeyValueTypesPairTemplate> modelControlPairTemplates) => new ControlsTemplate(modelControlPairTemplates.ToArray(), _modelTemplatePairTemplates, _controlTemplates);
			public ControlsTemplate WithModelTemplatePairTemplates(IEnumerable<KeyValueTypesPairTemplate> modelTemplatePairTemplates) => new ControlsTemplate(_modelControlPairTemplates, modelTemplatePairTemplates.ToArray(), _controlTemplates);
			public ControlsTemplate WithControlTemplates(IEnumerable<ControlTemplate> controlTemplates) => new ControlsTemplate(_modelControlPairTemplates, _modelTemplatePairTemplates, controlTemplates.ToArray());

			public String Build()
			{
				var controls = String.Join("\n\n", _controlTemplates.Select(t => t.Build()));
				var modelControlPairs = String.Join(",\n", _modelControlPairTemplates.Select(t => t.Build()));
				var modelTemplatePairs = String.Join(",\n", _modelTemplatePairTemplates.Select(t => t.Build()));

				var template = TEMPLATE
					.Replace(MODEL_CONTROL_PAIRS, modelControlPairs)
					.Replace(MODEL_TEMPLATE_PAIRS, modelTemplatePairs)
					.Replace(CONTROLS, controls);

				return template;
			}

			public override String ToString() => Build();
		}
	}
}