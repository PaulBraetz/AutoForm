using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
    public readonly struct Namespace : IEquatable<Namespace>
    {
        private Namespace(IdentifierPart[] parts)
        {
            Parts = parts ?? Array.Empty<IdentifierPart>();
            _string = String.Concat(Parts);
            _json = Json.Value(_string);
        }

        public static readonly Namespace Attributes = Create().With("AutoForm").With("Attributes");

        public readonly IdentifierPart[] Parts;
        private readonly String _json;
        private readonly String _string;

        public static Namespace Create()
        {
            return new Namespace(Array.Empty<IdentifierPart>());
        }
        public Namespace With(String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return this;
            }

            var parts = GetNextParts()
                .Append(IdentifierPart.Name(name))
                .ToArray();

            return new Namespace(parts);
        }
        public Namespace Prepend(String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return this;
            }

            var parts = GetPreviousParts()
                .Prepend(IdentifierPart.Name(name))
                .ToArray();

            return new Namespace(parts);
        }
        public Namespace PrependRange(IEnumerable<String> names)
        {
            var @namespace = this;
            foreach (var name in names)
            {
                @namespace = @namespace.Prepend(name);
            }

            return @namespace;
        }
        public Namespace WithRange(IEnumerable<String> names)
        {
            var @namespace = this;
            foreach (var name in names)
            {
                @namespace = @namespace.With(name);
            }

            return @namespace;
        }

        private IEnumerable<IdentifierPart> GetNextParts()
        {
            IEnumerable<IdentifierPart> parts = Parts ?? Array.Empty<IdentifierPart>();

            var lastKind = parts.LastOrDefault().Kind;

            var prependSeparator = lastKind == IdentifierPart.PartKind.Name;

            return prependSeparator ?
                parts.Append(IdentifierPart.Period()) :
                parts;
        }
        private IEnumerable<IdentifierPart> GetPreviousParts()
        {
            IEnumerable<IdentifierPart> parts = Parts ?? Array.Empty<IdentifierPart>();

            var firstKind = parts.FirstOrDefault().Kind;

            var appendSeparator = firstKind == IdentifierPart.PartKind.Name;

            return appendSeparator ?
                parts.Prepend(IdentifierPart.Period()) :
                parts;
        }

        public override String ToString()
        {
            return _json ?? "null";
        }
        public String ToEscapedString()
        {
            return _string ?? String.Empty;
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Namespace @namespace && Equals(@namespace);
        }

        public Boolean Equals(Namespace other)
        {
            return _json == other._json;
        }

        public override Int32 GetHashCode()
        {
            return -992964542 + EqualityComparer<String>.Default.GetHashCode(_json);
        }

        public static Boolean operator ==(Namespace left, Namespace right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(Namespace left, Namespace right)
        {
            return !(left == right);
        }
    }
}
