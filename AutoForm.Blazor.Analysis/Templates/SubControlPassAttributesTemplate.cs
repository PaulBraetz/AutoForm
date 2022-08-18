using System;

namespace AutoForm.Generate.Blazor.Templates
{
    internal sealed partial class SourceFactory
    {
        private readonly struct SubControlPassAttributesTemplate
        {
            private SubControlPassAttributesTemplate(String attributesProviderIdentifier)
            {
                _attributesProviderIdentifier = attributesProviderIdentifier;
            }

            private readonly String _attributesProviderIdentifier;

            private const String TEMPLATE =
    @"
                    __builder.AddAttribute(" + LINE_INDEX + @", ""Attributes"", global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<global::System.String, global::System.Object>>>(Value." + ATTRIBUTES_PROVIDER_IDENTIFIER + @".Get" + PROPERTY_IDENTIFIER + "Attributes()));";

            public SubControlPassAttributesTemplate WithAttributesProviderIdentifier(String attributesProviderIdentifier)
            {
                return new SubControlPassAttributesTemplate(attributesProviderIdentifier);
            }

            public String Build()
            {
                return String.IsNullOrEmpty(_attributesProviderIdentifier) ?
                    String.Empty :
                    TEMPLATE
                        .Replace(ATTRIBUTES_PROVIDER_IDENTIFIER, _attributesProviderIdentifier);
            }

            public override String ToString()
            {
                return Build();
            }
        }
    }
}