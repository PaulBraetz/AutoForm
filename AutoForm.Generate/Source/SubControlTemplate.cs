using System;
using System.Text.RegularExpressions;

namespace AutoForm.Generate
{
    internal static partial class Source
    {
        private readonly struct SubControlTemplate
        {
            private SubControlTemplate(String modelType, String propertyIdentifier, String propertyType, String subControlType)
            {
                _modelType = modelType;
                _propertyIdentifier = propertyIdentifier;
                _propertyType = propertyType;
                _subControlType = subControlType;
            }

            private readonly String _modelType;
            private readonly String _propertyIdentifier;
            private readonly String _propertyType;
            private readonly String _subControlType;

            private const String TEMPLATE =
@"					//SubControl for " + PROPERTY_IDENTIFIER + @"
					builder.OpenComponent<" + SUB_CONTROL_TYPE + @">(" + SUB_CONTROL_LINE_INDEX + @");
					builder.AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""Value"", RuntimeHelpers.TypeCheck<" + PROPERTY_TYPE + @">(Value." + PROPERTY_IDENTIFIER + @"));
					builder.AddAttribute(" + SUB_CONTROL_LINE_INDEX + @", ""ValueChanged"", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => Value." + PROPERTY_IDENTIFIER + @" = __value, Value." + PROPERTY_IDENTIFIER + @"))));
					builder.CloseComponent();";

            public SubControlTemplate WithModelType(String modelType)
            {
                return new SubControlTemplate(modelType, _propertyIdentifier, _propertyType, _subControlType);
            }
            public SubControlTemplate WithPropertyIdentifier(String propertyIdentifier)
            {
                return new SubControlTemplate(_modelType, propertyIdentifier, _propertyIdentifier, _subControlType);
            }
            public SubControlTemplate WithPropertyType(String propertyType)
            {
                return new SubControlTemplate(_modelType, _propertyIdentifier, propertyType, _subControlType);
            }
            public SubControlTemplate WithSubControlType(String subControlType)
            {
                return new SubControlTemplate(_modelType, _propertyIdentifier, _propertyType, subControlType);
            }

            public String Build(ref Int32 subControlLineIndex)
            {
                var template = TEMPLATE
                    .Replace(MODEL_TYPE, _modelType)
                    .Replace(PROPERTY_IDENTIFIER, _propertyIdentifier)
                    .Replace(PROPERTY_TYPE, _propertyType)
                    .Replace(SUB_CONTROL_TYPE, _subControlType);

                var regex = new Regex(Regex.Escape(SUB_CONTROL_LINE_INDEX));
                while (regex.IsMatch(template))
                {
                    template = regex.Replace(template, subControlLineIndex.ToString(), 1);
                    subControlLineIndex++;
                }

                return template;
            }

            public override String ToString()
            {
                return TEMPLATE
                    .Replace(MODEL_TYPE, _modelType)
                    .Replace(PROPERTY_IDENTIFIER, _propertyIdentifier)
                    .Replace(PROPERTY_TYPE, _propertyType)
                    .Replace(SUB_CONTROL_TYPE, _subControlType); 
            }
        }
    }

}
