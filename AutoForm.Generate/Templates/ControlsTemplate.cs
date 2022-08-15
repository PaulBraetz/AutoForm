﻿using System;
using System.Collections.Generic;
using System.Linq;
using static AutoForm.Generate.Blazor.BlazorSourceGenerator;

namespace AutoForm.Generate.Blazor.Templates
{
    internal readonly partial struct SourceFactory
    {
        private readonly struct ControlsTemplate
        {
            private ControlsTemplate(IEnumerable<ModelControlPairTemplate> modelControlPairTemplates, IEnumerable<ControlTemplate> controlTemplates, DefaultControlsTemplate defaultControlsTemplate)
            {
                _defaultControlsTemplate = defaultControlsTemplate;
                _modelControlPairTemplates = modelControlPairTemplates;
                _controlTemplates = controlTemplates;
            }

            private readonly DefaultControlsTemplate _defaultControlsTemplate;
            private readonly IEnumerable<ModelControlPairTemplate> _modelControlPairTemplates;
            private readonly IEnumerable<ControlTemplate> _controlTemplates;

            private const String TEMPLATE =
    @"// <auto-generated/>
// " + GENERATED_DATE + @"

#pragma warning disable 1591
using Microsoft.AspNetCore.Components;
namespace AutoForm.Generate.Blazor.Templates
{	
	public static class Controls
	{
#region ModelControlMap
		public static readonly IDictionary<Type, Type> ModelControlMap = new global::System.Collections.ObjectModel.ReadOnlyDictionary<Type, Type>(new Dictionary<Type, Type>()
		{
" + MODEL_CONTROL_PAIRS + @"
		});
#endregion

		private abstract class __ControlBase<TModel> : global::Microsoft.AspNetCore.Components.ComponentBase
		{				
			[global::Microsoft.AspNetCore.Components.Parameter]
			public TModel Value { get; set; }

			[global::Microsoft.AspNetCore.Components.Parameter]
			public global::Microsoft.AspNetCore.Components.EventCallback<TModel> ValueChanged { get; set; }

			[global::Microsoft.AspNetCore.Components.Parameter]
			public global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<global::System.String, global::System.Object>> Attributes { get; set; }
		}

#region Default Controls
" + DEFAULT_CONTROLS + @"
#endregion

#region Generated Controls
" + CONTROLS + @"
#endregion
	}
}
#pragma warning restore 1591";

            public ControlsTemplate WithModelControlPairTemplates(IEnumerable<ModelControlPairTemplate> modelControlPairTemplates)
            {
                return new ControlsTemplate(modelControlPairTemplates, _controlTemplates, _defaultControlsTemplate);
            }
            public ControlsTemplate WithControlTemplates(IEnumerable<ControlTemplate> controlTemplates)
            {
                return new ControlsTemplate(_modelControlPairTemplates, controlTemplates, _defaultControlsTemplate);
            }
            public ControlsTemplate WithDefaultControlsTemplate(DefaultControlsTemplate defaultControlsTemplate)
            {
                return new ControlsTemplate(_modelControlPairTemplates, _controlTemplates, defaultControlsTemplate);
            }

            public String Build()
            {
                String controls = String.Join("\n\n", _controlTemplates.Select(t => t.Build()));
                String modelControlPairs = String.Join(",\n", _modelControlPairTemplates.Select(t => t.Build()));
                String defaultControls = String.Join("\n\n", _defaultControlsTemplate.Build());
                String generatedDate = DateTimeOffset.Now.ToString();

                String template = TEMPLATE
                    .Replace(GENERATED_DATE, generatedDate)
                    .Replace(MODEL_CONTROL_PAIRS, modelControlPairs)
                    .Replace(DEFAULT_CONTROLS, defaultControls)
                    .Replace(CONTROLS, controls);

                template = ControlTypeIdentifierTemplate.Sanitize(template);

                return template;
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}