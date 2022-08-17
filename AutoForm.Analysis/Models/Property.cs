using System;
using System.Collections.Generic;

namespace AutoForm.Analysis.Models
{
    public readonly struct Property : IEquatable<Property>
    {
        public readonly Int32 Order;
        public readonly TypeIdentifier Control;
        public readonly TypeIdentifier Template;
        public readonly PropertyIdentifier Name;
        public readonly TypeIdentifier Type;
        private readonly String _json;
        private readonly String _string;

        private Property(PropertyIdentifier name, TypeIdentifier type, TypeIdentifier control, TypeIdentifier template, Int32 order)
        {
            Name = name;
            Type = type;
            Control = control;
            Template = template;
            Order = order;

            _json = Json.Object(Json.KeyValuePair(nameof(Order), Order),
                                                Json.KeyValuePair(nameof(Name), Name),
                                                Json.KeyValuePair(nameof(Type), Type),
                                                Json.KeyValuePair(nameof(Control), Control),
                                                Json.KeyValuePair(nameof(Template), Template));
            _string = _json;
        }
        public static Property Create(PropertyIdentifier identifier, TypeIdentifier type, TypeIdentifier control, TypeIdentifier template, Int32 order)
        {
            return new Property(identifier, type, control, template, order);
        }

        public override Boolean Equals(Object obj)
        {
            return obj is Property property && Equals(property);
        }

        public Boolean Equals(Property other)
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

        public static Boolean operator ==(Property left, Property right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(Property left, Property right)
        {
            return !(left == right);
        }
    }
}
