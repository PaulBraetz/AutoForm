using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Models
{
    public readonly struct Control : IEquatable<Control>
    {
        public readonly TypeIdentifier Name;
        public readonly IEnumerable<TypeIdentifier> Models;
        private readonly String _json;
        private readonly String _string;

        private Control(TypeIdentifier name, IEnumerable<TypeIdentifier> models)
        {
            models.ThrowOnDuplicate(TypeIdentifierName.AutoControlAttribute.ToString());

            Name = name;
            Models = models;

            _json = Json.Object(Json.KeyValuePair(nameof(Models), Models),
                                                Json.KeyValuePair(nameof(Name), Name));
            _string = _json;
        }

        public static Control Create(TypeIdentifier identifier)
        {
            return new Control(identifier, Array.Empty<TypeIdentifier>());
        }
        public Control Append(TypeIdentifier modelIdentifier)
        {
            return new Control(Name, Models.Append(modelIdentifier));
        }
        public Control AppendRange(IEnumerable<TypeIdentifier> modelIdentifiers)
        {
            return new Control(Name, Models.AppendRange(modelIdentifiers));
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Control control && Equals(control);
        }

        public Boolean Equals(Control other)
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

        public static Boolean operator ==(Control left, Control right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(Control left, Control right)
        {
            return !(left == right);
        }
    }
}
