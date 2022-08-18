using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoForm.Analysis.Models
{
    public readonly struct TypeIdentifierName : IEquatable<TypeIdentifierName>
    {
        private static readonly Regex AttributeRegex = new Regex(@"Attribute$");

        public static readonly TypeIdentifierName ModelAttribute = Create().AppendNamePart(GetAttributeName(typeof(Attributes.ModelAttribute)));

        public static readonly TypeIdentifierName ControlAttribute = Create().AppendNamePart(GetAttributeName(typeof(Attributes.UseControlAttribute)));
        public static readonly TypeIdentifierName TemplateAttribute = Create().AppendNamePart(GetAttributeName(typeof(Attributes.UseTemplateAttribute)));

        public static readonly TypeIdentifierName FallbackControlAttribute = Create().AppendNamePart(GetAttributeName(typeof(Attributes.FallbackControlAttribute)));
        public static readonly TypeIdentifierName FallbackTemplateAttribute = Create().AppendNamePart(GetAttributeName(typeof(Attributes.FallbackTemplateAttribute)));

        public static readonly TypeIdentifierName ExcludeAttribute = Create().AppendNamePart(GetAttributeName(typeof(Attributes.ExcludeAttribute)));
        public static readonly TypeIdentifierName AttributesProviderAttribute = Create().AppendNamePart(GetAttributeName(typeof(Attributes.AttributesProviderAttribute)));
        public static readonly TypeIdentifierName OrderAttribute = Create().AppendNamePart(GetAttributeName(typeof(Attributes.OrderAttribute)));
        
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
                .Concat(type.Parts);

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
                parts = parts.Concat(type.Namespace.Parts)
                    .Append(IdentifierPart.Period())
                    .Concat(type.Name.Parts);

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

        private static String GetAttributeName(Type attributeType)
        {
            return AttributeRegex.Replace(attributeType.Name, String.Empty);
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
