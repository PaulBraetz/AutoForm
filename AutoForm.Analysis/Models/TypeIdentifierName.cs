using AutoForm.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoForm.Analysis.Models
{
    public readonly struct TypeIdentifierName : IEquatable<TypeIdentifierName>
    {
        public static readonly TypeIdentifierName ModelAttribute = GetAttributeName<ModelAttribute>();

        public static readonly TypeIdentifierName ControlAttribute = GetAttributeName<UseControlAttribute>();
        public static readonly TypeIdentifierName TemplateAttribute = GetAttributeName<UseTemplateAttribute>();

        public static readonly TypeIdentifierName FallbackControlAttribute = GetAttributeName<FallbackControlAttribute>();
        public static readonly TypeIdentifierName FallbackTemplateAttribute = GetAttributeName<FallbackTemplateAttribute>();

        public static readonly TypeIdentifierName ExcludeAttribute = GetAttributeName<ExcludeAttribute>();
        public static readonly TypeIdentifierName AttributesProviderAttribute = GetAttributeName<AttributesProviderAttribute>();
        public static readonly TypeIdentifierName OrderAttribute = GetAttributeName<OrderAttribute>();

        public readonly IdentifierPart[] Parts;
        private readonly String _json;
        private readonly String _string;

        private TypeIdentifierName(IdentifierPart[] parts)
        {
            Parts = parts ?? Array.Empty<IdentifierPart>();

            _string = String.Concat(parts);
            _json = Json.Value(_string);
        }

        private static TypeIdentifierName GetAttributeName<T>()
        {
            return TypeIdentifierName.Create().WithNamePart(Regex.Replace(typeof(T).Name, @"Attribute$", String.Empty));
        }

        public static TypeIdentifierName Create()
        {
            return new TypeIdentifierName(Array.Empty<IdentifierPart>());
        }
        public TypeIdentifierName WithTypePart(TypeIdentifierName type)
        {
            var parts = GetNextParts(IdentifierPart.PartKind.Name)
                .Concat(type.Parts)
                .ToArray();

            return new TypeIdentifierName(parts);
        }
        public TypeIdentifierName WithNamePart(String name)
        {
            var parts = GetNextParts(IdentifierPart.PartKind.Name)
                .Append(IdentifierPart.Name(name))
                .ToArray();

            return new TypeIdentifierName(parts);
        }
        public TypeIdentifierName WithGenericPart(TypeIdentifier[] arguments)
        {
            var parts = GetNextParts(IdentifierPart.PartKind.GenericOpen)
                .Append(IdentifierPart.GenericOpen());

            var typesArray = arguments ?? Array.Empty<TypeIdentifier>();

            for (var i = 0; i < typesArray.Length; i++)
            {
                var type = typesArray[i];

                if (type.Namespace.Parts.Any())
                {
                    parts = parts.Concat(type.Namespace.Parts)
                                 .Append(IdentifierPart.Period());
                }

                parts = parts.Concat(type.Name.Parts);

                if (i != typesArray.Length - 1)
                {
                    parts = parts.Append(IdentifierPart.Comma());
                }
            }

            parts = parts.Append(IdentifierPart.GenericClose());

            return new TypeIdentifierName(parts.ToArray());
        }
        public TypeIdentifierName WithArrayPart()
        {
            var parts = GetNextParts(IdentifierPart.PartKind.Array)
                .Append(IdentifierPart.Array())
                .ToArray();

            return new TypeIdentifierName(parts);
        }

        private IEnumerable<IdentifierPart> GetNextParts(IdentifierPart.PartKind nextKind)
        {
            var parts = Parts ?? Array.Empty<IdentifierPart>();

            var lastKind = parts.LastOrDefault().Kind;
            var prependSeparator = nextKind == IdentifierPart.PartKind.Name &&
                                        (lastKind == IdentifierPart.PartKind.GenericOpen ||
                                        lastKind == IdentifierPart.PartKind.Name);

            if (prependSeparator)
            {
                return parts.Append(IdentifierPart.Period());
            }

            return parts;
        }

        public override Boolean Equals(Object obj)
        {
            return obj is TypeIdentifierName identifier && Equals(identifier);
        }

        public Boolean Equals(TypeIdentifierName other)
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

        public static Boolean operator ==(TypeIdentifierName left, TypeIdentifierName right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(TypeIdentifierName left, TypeIdentifierName right)
        {
            return !(left == right);
        }
    }
}
