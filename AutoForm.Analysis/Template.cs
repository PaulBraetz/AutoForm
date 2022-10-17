using AutoForm.Analysis;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
    internal readonly struct Template : IEquatable<Template>
    {
        public readonly TypeIdentifier Name;
        public readonly TypeIdentifier[] Models;

        private Template(TypeIdentifier name, TypeIdentifier[] models)
        {
            models.ThrowOnDuplicate(TypeIdentifierName.FallbackTemplateAttribute.ToString());

            Name = name;
            Models = models ?? Array.Empty<TypeIdentifier>();
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

        public override bool Equals(object obj)
        {
            return obj is Template template && Equals(template);
        }

        public bool Equals(Template other)
        {
            return _json == other._json;
        }

        public override int GetHashCode()
        {
            return -992964542 + EqualityComparer<string>.Default.GetHashCode(_json);
        }

        public static bool operator ==(Template left, Template right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Template left, Template right)
        {
            return !(left == right);
        }
    }
}
