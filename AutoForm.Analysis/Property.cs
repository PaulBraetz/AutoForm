using System;
using System.Collections.Generic;

namespace AutoForm.Analysis
{
    internal readonly struct Property : IEquatable<Property>
    {
        public readonly int Order;
        public readonly TypeIdentifier Control;
        public readonly TypeIdentifier Template;
        public readonly PropertyIdentifier Name;
        public readonly TypeIdentifier Type;
        private readonly string _json;
        private readonly string _string;

        private Property(PropertyIdentifier name, TypeIdentifier type, TypeIdentifier control, TypeIdentifier template, int order)
        {
            Name = name;
            Type = type;
            Control = control;
            Template = template;
            Order = order;

            _json = Json.Object(Json.KeyValuePair(nameof(Name), Name),
                                Json.KeyValuePair(nameof(Type), Type),
                                Json.KeyValuePair(nameof(Order), Order),
                                Json.KeyValuePair(nameof(Control), Control),
                                Json.KeyValuePair(nameof(Template), Template));
            _string = _json;
        }
        public static Property Create(PropertyIdentifier name, TypeIdentifier type, TypeIdentifier control, TypeIdentifier template, int order)
        {
            return new Property(name, type, control, template, order);
        }

        public override bool Equals(object obj)
        {
            return obj is Property property && Equals(property);
        }

        public bool Equals(Property other)
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

        public static bool operator ==(Property left, Property right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Property left, Property right)
        {
            return !(left == right);
        }
    }
}
