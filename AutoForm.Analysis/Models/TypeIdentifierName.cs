﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
    public readonly struct TypeIdentifierName : IEquatable<TypeIdentifierName>
    {
        public static readonly TypeIdentifierName AutoControlAttribute = Create().AppendNamePart("AutoControl");
        public static readonly TypeIdentifierName AutoControlAttributesProviderAttribute = Create().AppendNamePart("AutoControlAttributesProvider");
        public static readonly TypeIdentifierName AutoControlModelAttribute = Create().AppendNamePart("AutoControlModel");
        public static readonly TypeIdentifierName AutoControlPropertyControlAttribute = Create().AppendNamePart("AutoControlPropertyControl");
        public static readonly TypeIdentifierName AutoControlPropertyTemplateAttribute = Create().AppendNamePart("AutoControlPropertyTemplate");
        public static readonly TypeIdentifierName AutoControlPropertyExcludeAttribute = Create().AppendNamePart("AutoControlPropertyExclude");
        public static readonly TypeIdentifierName AutoControlPropertyOrderAttribute = Create().AppendNamePart("AutoControlPropertyOrder");
        public static readonly TypeIdentifierName AutoControlTemplateAttribute = Create().AppendNamePart("AutoControlTemplate");

        public readonly IEnumerable<IdentifierPart> Parts;
        private readonly String _json;
        private readonly String _string;

        private TypeIdentifierName(IEnumerable<IdentifierPart> parts)
        {
            Parts = parts;

            _string = String.Concat(parts);
            _json = Json.Value(_string);
        }
        public static TypeIdentifierName Create()
        {
            return new TypeIdentifierName(Array.Empty<IdentifierPart>());
        }
        public TypeIdentifierName AppendTypePart(TypeIdentifierName type)
        {
            IEnumerable<IdentifierPart> parts = GetNextParts(IdentifierPart.PartKind.Name)
                .AppendRange(type.Parts);

            return new TypeIdentifierName(parts);
        }
        public TypeIdentifierName AppendNamePart(String name)
        {
            IEnumerable<IdentifierPart> parts = GetNextParts(IdentifierPart.PartKind.Name)
                .Append(IdentifierPart.Name(name));

            return new TypeIdentifierName(parts);
        }
        public TypeIdentifierName AppendGenericPart(IEnumerable<TypeIdentifier> arguments)
        {
            IEnumerable<IdentifierPart> parts = GetNextParts(IdentifierPart.PartKind.GenericOpen)
                .Append(IdentifierPart.GenericOpen());

            TypeIdentifier[] typesArray = arguments.ToArray();

            for (Int32 i = 0; i < typesArray.Length; i++)
            {
                var type = typesArray[i];
                parts = parts.AppendRange(type.Namespace.Parts)
                    .Append(IdentifierPart.Period())
                    .AppendRange(type.Name.Parts);

                if (i != typesArray.Length - 1)
                {
                    parts = parts.Append(IdentifierPart.Comma());
                }
            }

            parts = parts.Append(IdentifierPart.GenericClose());

            return new TypeIdentifierName(parts);
        }
        public TypeIdentifierName AppendArrayPart()
        {
            IEnumerable<IdentifierPart> parts = GetNextParts(IdentifierPart.PartKind.Array)
                .Append(IdentifierPart.Array());

            return new TypeIdentifierName(parts);
        }

        private IEnumerable<IdentifierPart> GetNextParts(IdentifierPart.PartKind nextKind)
        {
            var parts = Parts ?? Array.Empty<IdentifierPart>();

            IdentifierPart.PartKind lastKind = parts.LastOrDefault().Kind;
            Boolean prependSeparator = nextKind == IdentifierPart.PartKind.Name &&
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