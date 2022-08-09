using System;
using System.Text.RegularExpressions;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct SubControlTemplate
		{
			private SubControlTemplate(String modelType,
									   String propertyIdentifier,
									   String propertyType,
									   String subControlType,
									   SubControlPropertyIdentifierTemplate subControlPropertyIdentifierTemplate)
			{
				_modelType = modelType;
				_propertyIdentifier = propertyIdentifier;
				_propertyType = propertyType;
				_subControlType = subControlType;
				_subControlPropertyIdentifierTemplate = subControlPropertyIdentifierTemplate;
			}

			private readonly String _modelType;
			private readonly String _propertyIdentifier;
			private readonly String _propertyType;
			private readonly String _subControlType;

			private readonly SubControlPropertyIdentifierTemplate _subControlPropertyIdentifierTemplate;

			private const String TEMPLATE =
@"					    //SubControl for " + PROPERTY_IDENTIFIER + @"
                        __builder.OpenComponent<" + SUB_CONTROL_TYPE + @">(" + SUB_CONTROL_LINE_INDEX + @");
                        __builder.AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""Value"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(
#nullable restore
                            " + SUB_CONTROL_PROPERTY_IDENTIFIER + @"
#nullable disable
                        ));
                        __builder.AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""ValueChanged"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::Microsoft.AspNetCore.Components.EventCallback<" + PROPERTY_TYPE + @">>(global::Microsoft.AspNetCore.Components.EventCallback.Factory.Create<" + PROPERTY_TYPE + @">(this, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => { " + SUB_CONTROL_PROPERTY_IDENTIFIER + @" = __value; return ValueChanged.InvokeAsync(Value);}, " + SUB_CONTROL_PROPERTY_IDENTIFIER + @"))));
                        __builder.CloseComponent();";

			public SubControlTemplate WithModelType(String modelType)
			{
				return new SubControlTemplate(modelType,
											  _propertyIdentifier,
											  _propertyType,
											  _subControlType,
											  _subControlPropertyIdentifierTemplate);
			}
			public SubControlTemplate WithPropertyIdentifier(String propertyIdentifier)
			{
				return new SubControlTemplate(_modelType,
											  propertyIdentifier,
											  _propertyIdentifier,
											  _subControlType,
											  _subControlPropertyIdentifierTemplate);
			}
			public SubControlTemplate WithPropertyType(String propertyType)
			{
				return new SubControlTemplate(_modelType,
											  _propertyIdentifier,
											  propertyType,
											  _subControlType,
											  _subControlPropertyIdentifierTemplate);
			}
			public SubControlTemplate WithSubControlType(String subControlType)
			{
				return new SubControlTemplate(_modelType,
											  _propertyIdentifier,
											  _propertyType,
											  subControlType,
											  _subControlPropertyIdentifierTemplate);
			}
			public SubControlTemplate WithSubControlPropertyIdentifierTemplate(SubControlPropertyIdentifierTemplate subControlPropertyIdentifierTemplate)
			{
				return new SubControlTemplate(_modelType,
											  _propertyIdentifier,
											  _propertyType,
											  _subControlType,
											  subControlPropertyIdentifierTemplate);
			}

			public String Build(ref Int32 subControlLineIndex)
			{
				var subControlPropertyIdentifier = _subControlPropertyIdentifierTemplate.Build();

				var template = TEMPLATE
					.Replace(MODEL_TYPE, _modelType)
					.Replace(PROPERTY_IDENTIFIER, _propertyIdentifier)
					.Replace(PROPERTY_TYPE, _propertyType)
					.Replace(SUB_CONTROL_TYPE, _subControlType)
					.Replace(SUB_CONTROL_PROPERTY_IDENTIFIER, subControlPropertyIdentifier);

				var regex = new Regex(Regex.Escape(SUB_CONTROL_LINE_INDEX));
				while (regex.IsMatch(template))
				{
					template = regex.Replace(template, subControlLineIndex.ToString(), 1);
					subControlLineIndex++;
				}

				return template;
			}


		}
	}

}
