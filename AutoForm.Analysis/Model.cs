using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
    internal readonly struct Model : IEquatable<Model>
    {
        public readonly TypeIdentifier Name;
        public readonly TypeIdentifier Control;
        public readonly TypeIdentifier Template;
        public readonly PropertyIdentifier AttributesProvider;
        public readonly Property[] Properties;
        private readonly string _json;
        private readonly string _string;

        private Model(TypeIdentifier name, TypeIdentifier control, TypeIdentifier template, Property[] properties, PropertyIdentifier attributesProvider)
        {
            properties.ThrowOnDuplicate(nameof(properties));

            Name = name;
            Control = control;
            Template = template;
            Properties = properties ?? Array.Empty<Property>();
            AttributesProvider = attributesProvider;

            _json = Json.Object(Json.KeyValuePair(nameof(Name), Name),
                                Json.KeyValuePair(nameof(AttributesProvider), AttributesProvider),
                                Json.KeyValuePair(nameof(Control), Control),
                                Json.KeyValuePair(nameof(Template), Template),
                                Json.KeyValuePair(nameof(Properties), Properties));
            _string = _json;
        }

        public static Model Create(TypeIdentifier name, TypeIdentifier control, TypeIdentifier template, PropertyIdentifier attributesProvider)
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

        public override bool Equals(object obj)
        {
            return obj is Model type && Equals(type);
        }

        public bool Equals(Model other)
        {
            return _json == other._json;
        }

        public override int GetHashCode()
        {
            return -992964542 + EqualityComparer<string>.Default.GetHashCode(_json);
        }

        public override string ToString()
        {
            return _json ?? "null";
        }
        public string ToEscapedString()
        {
            return _string ?? string.Empty;
        }

        public static bool operator ==(Model left, Model right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Model left, Model right)
        {
            return !(left == right);
        }
    }
}
