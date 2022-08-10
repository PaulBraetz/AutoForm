using System;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct SubControlPropertyTemplate
		{
			public SubControlPropertyTemplate(String propertyType, String propertyIdentifier, SubControlPropertyIdentifierTemplate subControlPropertyIdentifierTemplate)
			{
				_propertyType = propertyType;
				_propertyIdentifier = propertyIdentifier;
				_subControlPropertyIdentifierTemplate = subControlPropertyIdentifierTemplate;
			}

			private readonly String _propertyType;
			private readonly String _propertyIdentifier;
			private readonly SubControlPropertyIdentifierTemplate _subControlPropertyIdentifierTemplate;

			private const String TEMPLATE =
@"            private " + PROPERTY_TYPE + @" " + SUB_CONTROL_PROPERTY_IDENTIFIER + @"
            {
                get => Value!." + PROPERTY_IDENTIFIER + @";
                set
                {
                    Value!." + PROPERTY_IDENTIFIER + @" = value;
                    ValueChanged.InvokeAsync(Value);
                }
            }";

			public SubControlPropertyTemplate WithPropertyType(String propertyType)
			{
				return new SubControlPropertyTemplate(propertyType, _propertyIdentifier, _subControlPropertyIdentifierTemplate);
			}
			public SubControlPropertyTemplate WithPropertyIdentifier(String propertyIdentifier)
			{
				return new SubControlPropertyTemplate(_propertyIdentifier, propertyIdentifier, _subControlPropertyIdentifierTemplate);
			}
			public SubControlPropertyTemplate WithSubControlPropertyIdentifierTemplate(SubControlPropertyIdentifierTemplate subControlPropertyIdentifierTemplate)
			{
				return new SubControlPropertyTemplate(_propertyType, _propertyIdentifier, subControlPropertyIdentifierTemplate);
			}

			public String Build()
			{
				var subControlPropertyIdentifier = _subControlPropertyIdentifierTemplate.Build();

				return TEMPLATE
					.Replace(PROPERTY_TYPE, _propertyType)
					.Replace(PROPERTY_IDENTIFIER, _propertyIdentifier)
					.Replace(SUB_CONTROL_PROPERTY_IDENTIFIER, subControlPropertyIdentifier);
			}


		}
	}

}
