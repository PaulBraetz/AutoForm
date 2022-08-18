using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
    public readonly struct FallbackTemplate : IEquatable<FallbackTemplate>
    {
        public readonly TypeIdentifier Name;
        public readonly IEnumerable<TypeIdentifier> Models;
        private readonly String _json;
        private readonly String _string;

        private FallbackTemplate(TypeIdentifier name, IEnumerable<TypeIdentifier> models)
        {
            models.ThrowOnDuplicate(TypeIdentifierName.FallbackTemplateAttribute.ToString());

            Name = name;
            Models = models;

            _json = Json.Object(Json.KeyValuePair(nameof(Name), Name),
                                Json.KeyValuePair(nameof(Models), Models));
            _string = _json;
        }

        public static FallbackTemplate Create(TypeIdentifier identifier)
        {
            return new FallbackTemplate(identifier, Array.Empty<TypeIdentifier>());
        }
        public FallbackTemplate Append(TypeIdentifier modelIdentifier)
        {
            return new FallbackTemplate(Name, Models.Append(modelIdentifier));
        }
        public FallbackTemplate AppendRange(IEnumerable<TypeIdentifier> modelIdentifiers)
        {
            return new FallbackTemplate(Name, Models.AppendRange(modelIdentifiers));
        }

        public override Boolean Equals(Object obj)
        {
            return obj is FallbackTemplate template && Equals(template);
        }

        public Boolean Equals(FallbackTemplate other)
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

        public static Boolean operator ==(FallbackTemplate left, FallbackTemplate right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(FallbackTemplate left, FallbackTemplate right)
        {
            return !(left == right);
        }
    }
}
