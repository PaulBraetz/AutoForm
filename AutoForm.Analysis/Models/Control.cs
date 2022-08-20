using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
    public readonly struct Control : IEquatable<Control>
    {
        public readonly TypeIdentifier Name;
        public readonly TypeIdentifier[] Models;
        private readonly String _json;
        private readonly String _string;

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
