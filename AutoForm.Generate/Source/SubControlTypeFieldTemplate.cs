using System;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct SubControlTypeFieldTemplate
		{
			private SubControlTypeFieldTemplate(String modelType, String propertyIdentifier, String propertyType, SubControlFieldIdentifierTemplate subControlFieldIdentifierTemplate)
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
@"			private static readonly Type " + SUB_CONTROL_TYPE_FIELD_IDENTIFIER_TEMPLATE + @" = GetSubControlType(typeof(" + PROPERTY_TYPE + @"), nameof(" + MODEL_TYPE + @"." + PROPERTY_IDENTIFIER + @"), typeof(" + MODEL_TYPE + @"));";

			public SubControlTypeFieldTemplate WithModelType(String modelType)
			{
				return new SubControlTypeFieldTemplate(modelType, PropertyIdentifier, PropertyType, SubControlFieldIdentifierTemplate);
			}
			public SubControlTypeFieldTemplate WithPropertyIdentifier(String propertyIdentifier)
			{
				return new SubControlTypeFieldTemplate(ModelType, propertyIdentifier, PropertyIdentifier, SubControlFieldIdentifierTemplate);
			}
			public SubControlTypeFieldTemplate WithPropertyType(String propertyType)
			{
				return new SubControlTypeFieldTemplate(ModelType, PropertyIdentifier, propertyType, SubControlFieldIdentifierTemplate);
			}
			public SubControlTypeFieldTemplate WithSubControlFieldIdentifierTemplate(SubControlFieldIdentifierTemplate subControlFieldIdentifierTemplate)
			{
				return new SubControlTypeFieldTemplate(ModelType, PropertyIdentifier, PropertyType, subControlFieldIdentifierTemplate);
			}

			public String Build(ref Int32 subControlTypeFieldIndex)
			{
				return TEMPLATE
					.Replace(MODEL_TYPE, ModelType)
					.Replace(PROPERTY_IDENTIFIER, PropertyIdentifier)
					.Replace(PROPERTY_TYPE, PropertyType)
					.Replace(SUB_CONTROL_TYPE_FIELD_IDENTIFIER_TEMPLATE, SubControlFieldIdentifierTemplate.Build(ref subControlTypeFieldIndex));
			}
		}
	}
}
