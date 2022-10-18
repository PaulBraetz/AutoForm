using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
    internal readonly struct Model
    {
        public readonly ITypeIdentifier Name;
        public readonly ITypeIdentifier Control;
        public readonly ITypeIdentifier Template;
        public readonly PropertyIdentifier AttributesProvider;
        public readonly Property[] Properties;

        private Model(ITypeIdentifier name, ITypeIdentifier control, ITypeIdentifier template, Property[] properties, PropertyIdentifier attributesProvider)
        {
            properties.ThrowOnDuplicate(nameof(properties));

            Name = name;
            Control = control;
            Template = template;
            Properties = properties ?? Array.Empty<Property>();
            AttributesProvider = attributesProvider;
        }

        public static Model Create(ITypeIdentifier name, ITypeIdentifier control, ITypeIdentifier template, PropertyIdentifier attributesProvider)
        {
            return new Model(name, control, template, Array.Empty<Property>(), attributesProvider);
        }

        public Model With(Property property)
        {
            return new Model(Name, Control, Template, Properties.Append(property).ToArray(), AttributesProvider);
        }
        public Model WithRange(IEnumerable<Property> properties)
        {
            return new Model(Name, Control, Template, Properties.Concat(properties).ToArray(), AttributesProvider);
        }
    }
}
