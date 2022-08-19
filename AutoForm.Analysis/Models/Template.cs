using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
    public readonly struct Template : IEquatable<Template>
    {
        public readonly TypeIdentifier Name;
        public readonly TypeIdentifier[] Models;
        private readonly String _json;
        private readonly String _string;

        private Template(TypeIdentifier name, TypeIdentifier[] models)
        {
            models.ThrowOnDuplicate(TypeIdentifierName.FallbackTemplateAttribute.ToString());

            Name = name;
            Models = models ?? Array.Empty<TypeIdentifier>();

            _json = Json.Object(Json.KeyValuePair(nameof(Name), Name),
                                Json.KeyValuePair(nameof(Models), Models));
            _string = _json;
        }

        public static Template Create(TypeIdentifier identifier)
        {
            return new Template(identifier, Array.Empty<TypeIdentifier>());
        }
        public Template With(TypeIdentifier modelIdentifier)
        {
            return new Template(Name, Models.Append(modelIdentifier).ToArray());
        }
        public Template WithRange(IEnumerable<TypeIdentifier> modelIdentifiers)
        {
            return new Template(Name, Models.Concat(modelIdentifiers).ToArray());
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Template template && Equals(template);
        }

        public Boolean Equals(Template other)
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

        public static Boolean operator ==(Template left, Template right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(Template left, Template right)
        {
            return !(left == right);
        }
    }
}
