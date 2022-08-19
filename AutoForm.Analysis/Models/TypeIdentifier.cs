using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
	public readonly struct TypeIdentifier : IEquatable<TypeIdentifier>
	{
		public static readonly TypeIdentifier FallbackControlAttribute = Create(TypeIdentifierName.FallbackControlAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier AttributesProviderAttribute = Create(TypeIdentifierName.AttributesProviderAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier ModelAttribute = Create(TypeIdentifierName.ModelAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier TemplateAttribute = Create(TypeIdentifierName.TemplateAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier ControlAttribute = Create(TypeIdentifierName.ControlAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier ExcludeAttribute = Create(TypeIdentifierName.ExcludeAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier OrderAttribute = Create(TypeIdentifierName.OrderAttribute, Namespace.Attributes);
		public static readonly TypeIdentifier FallbackTemplateAttribute = Create(TypeIdentifierName.FallbackTemplateAttribute, Namespace.Attributes);

		public readonly TypeIdentifierName Name;
		public readonly Namespace Namespace;
		public readonly Boolean IsNotGenerated;
		private readonly String _json;
		private readonly String _string;

		private TypeIdentifier(TypeIdentifierName name, Namespace @namespace, Boolean isNotGenerated)
		{
			Name = name;
			Namespace = @namespace;
			IsNotGenerated = isNotGenerated;

			String namespaceString = Namespace.ToEscapedString();
			String nameString = Name.ToEscapedString();

			_string = String.IsNullOrEmpty(namespaceString) ? String.IsNullOrEmpty(nameString) ? null : nameString.ToString() : $"{namespaceString}.{nameString}";
			_json = Json.Value(_string);
		}

		public static TypeIdentifier CreateGeneratedControl(TypeIdentifier typeIdentifier)
		{
			var generatedName = $"__Control_{typeIdentifier.ToEscapedString().Replace(".", "_")}";
			var name = TypeIdentifierName.Create().WithNamePart(generatedName);
			var @namespace = Namespace.Create();

			return Create(name, @namespace, false);
		}
		public static TypeIdentifier Create<T>()
		{
			return Create(typeof(T));
		}
		public static TypeIdentifier Create(Type type)
		{
			TypeIdentifierName name = TypeIdentifierName.Create();
			Namespace @namespace = default;

			if (type.IsNested)
			{
				Type parentType = type.Assembly.GetTypes().Single(t => t.GetNestedType(type.FullName) != null);
				TypeIdentifier parentTypeIdentifier = Create(parentType);
				name = name.WithTypePart(parentTypeIdentifier.Name);
				@namespace = parentTypeIdentifier.Namespace;
			}

			name = name.WithNamePart(type.Name);

			if (type.IsConstructedGenericType)
			{
				var genericArguments = type.GenericTypeArguments.Select(Create).ToArray();
				name = name.WithGenericPart(genericArguments);
			}

			if (type.IsArray)
			{
				name = name.WithArrayPart();
			}

			if (@namespace == default)
			{
				String[] namespaceParts = type.Namespace.Split('.');
				@namespace = Namespace.Create().WithRange(namespaceParts);
			}

			return Create(name, @namespace);

		}

		private static TypeIdentifier Create(TypeIdentifierName name, Namespace @namespace, Boolean isNotGenerated)
		{
			return new TypeIdentifier(name, @namespace, isNotGenerated);
		}
		public static TypeIdentifier Create(TypeIdentifierName name, Namespace @namespace)
		{
			return Create(name, @namespace, true);
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
			return obj is TypeIdentifier identifier && Equals(identifier);
		}

		public Boolean Equals(TypeIdentifier other)
		{
			return _json == other._json;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_json);
		}

		public static Boolean operator ==(TypeIdentifier left, TypeIdentifier right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(TypeIdentifier left, TypeIdentifier right)
		{
			return !(left == right);
		}
	}
}
