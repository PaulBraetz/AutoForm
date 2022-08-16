using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Models
{
    public readonly struct Model : IEquatable<Model>
    {
        public readonly TypeIdentifier Name;
        public readonly PropertyIdentifier AttributesProvider;
        public readonly IEnumerable<Property> Properties;
        private readonly String _json;
        private readonly String _string;

        private Model(TypeIdentifier name, IEnumerable<Property> properties, PropertyIdentifier attributesProvider)
        {
            Name = name;
            Properties = properties;
            AttributesProvider = attributesProvider;

            _json = Json.Object(Json.KeyValuePair(nameof(Name), Name),
                                                Json.KeyValuePair(nameof(AttributesProvider), AttributesProvider),
                                                Json.KeyValuePair(nameof(Properties), Properties.Select(p => p)));
            _string = _json;
        }

        public static Model Create(TypeIdentifier identifier, PropertyIdentifier attributesProvider)
        {
            return new Model(identifier, Array.Empty<Property>(), attributesProvider);
        }
        public static Model Create(TypeIdentifier identifier)
        {
            return Create(identifier, default);
        }
        public Model Append(Property property)
        {
            return new Model(Name, Properties.Append(property), AttributesProvider);
        }
        public Model AppendRange(IEnumerable<Property> properties)
        {
            return new Model(Name, Properties.AppendRange(properties), AttributesProvider);
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Model type && Equals(type);
        }

        public Boolean Equals(Model other)
        {
            return _json == other._json;
        }

        public override Int32 GetHashCode()
        {
            return -992964542 + EqualityComparer<String>.Default.GetHashCode(_json);
        }

        public override String ToString()
        {
            return _json ?? "null";
        }
        public String ToEscapedString()
        {
            return _string ?? String.Empty;
        }

        public static Boolean operator ==(Model left, Model right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(Model left, Model right)
        {
            return !(left == right);
        }
    }
}
