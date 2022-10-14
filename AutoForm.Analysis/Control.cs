using System;
using System.Collections.Generic;
using System.Linq;
using AutoForm.Analysis;

namespace AutoForm.Analysis
{
    internal readonly struct Control : IEquatable<Control>
    {
        public readonly TypeIdentifier Name;
        public readonly TypeIdentifier[] Models;
        private readonly string _json;
        private readonly string _string;

        private Control(TypeIdentifier name, TypeIdentifier[] models)
        {
            models.ThrowOnDuplicate(TypeIdentifierName.FallbackControlAttribute.ToString());

            Name = name;
            Models = models ?? Array.Empty<TypeIdentifier>();

            _json = Json.Object(Json.KeyValuePair(nameof(Name), Name),
                                Json.KeyValuePair(nameof(Models), Models));
            _string = _json;
        }

        public static Control CreateGenerated(TypeIdentifier modelType)
        {
            var identifier = TypeIdentifier.CreateGeneratedControl(modelType);

            var control = Create(identifier)
                .With(modelType);

            return control;
        }

        public static Control Create(TypeIdentifier identifier)
        {
            return new Control(identifier, Array.Empty<TypeIdentifier>());
        }
        public Control With(TypeIdentifier modelIdentifier)
        {
            return new Control(Name, Models.Append(modelIdentifier).ToArray());
        }
        public Control WithRange(IEnumerable<TypeIdentifier> modelIdentifiers)
        {
            return new Control(Name, Models.Concat(modelIdentifiers).ToArray());
        }

        public override bool Equals(object obj)
        {
            return obj is Control control && Equals(control);
        }

        public bool Equals(Control other)
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

        public static bool operator ==(Control left, Control right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Control left, Control right)
        {
            return !(left == right);
        }
    }
}
