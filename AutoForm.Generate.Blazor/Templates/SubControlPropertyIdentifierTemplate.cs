using System;

namespace AutoForm.Generate.Blazor.Templates
{
	internal sealed partial class SourceFactory
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
				return _propertyIdentifier != null ?
					TEMPLATE.Replace(PROPERTY_IDENTIFIER, _propertyIdentifier) :
					null;
			}

			public override String ToString()
			{
				return Build();
			}
		}
	}
}