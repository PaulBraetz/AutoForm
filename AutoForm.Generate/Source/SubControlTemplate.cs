using System;
using System.Text.RegularExpressions;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct SubControlTemplate
		{
			private SubControlTemplate(String modelType, String propertyIdentifier, String propertyType, SubControlFieldIdentifierTemplate subControlFieldIdentifierTemplate)
			{
				ModelType = modelType;
				PropertyIdentifier = propertyIdentifier;
				PropertyType = propertyType;
				SubControlFieldIdentifierTemplate = subControlFieldIdentifierTemplate;
			}

			public readonly String ModelType;
			public readonly String PropertyIdentifier;
			public readonly String PropertyType;

			public readonly SubControlFieldIdentifierTemplate SubControlFieldIdentifierTemplate;

			private const String TEMPLATE =
@"					#region SubControl for property " + MODEL_TYPE + @"." + PROPERTY_IDENTIFIER + @"
					builder.OpenComponent(" + SUB_CONTROL_LINE_INDEX + @", " + SUB_CONTROL_TYPE_FIELD_IDENTIFIER_TEMPLATE + @");
					builder.AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""Value"", RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value." + PROPERTY_IDENTIFIER + @"));
					builder.AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""ValueChanged"", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => Value." + PROPERTY_IDENTIFIER + @" = __value, Value." + PROPERTY_IDENTIFIER + @"))));
					builder.CloseComponent();
					#endregion";

			public SubControlTemplate WithModelType(String modelType)
			{
				return new SubControlTemplate(modelType, PropertyIdentifier, PropertyType, SubControlFieldIdentifierTemplate);
			}
			public SubControlTemplate WithPropertyIdentifier(String propertyIdentifier)
			{
				return new SubControlTemplate(ModelType, propertyIdentifier, PropertyIdentifier, SubControlFieldIdentifierTemplate);
			}
			public SubControlTemplate WithPropertyType(String propertyType)
			{
				return new SubControlTemplate(ModelType, PropertyIdentifier, propertyType, SubControlFieldIdentifierTemplate);
			}
			public SubControlTemplate WithSubControlFieldIdentifierTemplate(SubControlFieldIdentifierTemplate subControlFieldIdentifierTemplate)
			{
				return new SubControlTemplate(ModelType, PropertyIdentifier, PropertyType, subControlFieldIdentifierTemplate);
			}

			public String Build(ref Int32 subControlLineIndex, ref Int32 subControlTypeFieldIndex)
			{
				var template = TEMPLATE
					.Replace(MODEL_TYPE, ModelType)
					.Replace(PROPERTY_IDENTIFIER, PropertyIdentifier)
					.Replace(PROPERTY_TYPE, PropertyType)
					.Replace(SUB_CONTROL_TYPE_FIELD_IDENTIFIER_TEMPLATE, SubControlFieldIdentifierTemplate.Build(ref subControlTypeFieldIndex));

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
