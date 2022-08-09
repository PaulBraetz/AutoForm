using System;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct SubControlPropertyIdentifierTemplate
		{
			public SubControlPropertyIdentifierTemplate(String propertyIdentifier)
			{
				_propertyIdentifier = propertyIdentifier;
			}

			private readonly String _propertyIdentifier;

			private const String TEMPLATE = "__" + PROPERTY_IDENTIFIER;

			public SubControlPropertyIdentifierTemplate WithPropertyIdentifier(String propertyIdentifier)
			{
				return new SubControlPropertyIdentifierTemplate(propertyIdentifier);
			}

			public String Build()
			{
				return TEMPLATE
					.Replace(PROPERTY_IDENTIFIER, _propertyIdentifier);
			}
		}
	}

}
