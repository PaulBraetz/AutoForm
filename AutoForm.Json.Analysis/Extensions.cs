using AutoForm.Analysis;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Json.Analysis
{
	internal static class Extensions
	{
		public static IJsonDecorator<IdentifierPart> AsJson(this IdentifierPart part)
		{
			return new IdentifierPartJsonDecorator(part);
		}
		public static IJsonDecorator<TypeIdentifierName> AsJson(this TypeIdentifierName name)
		{
			return new TypeIdentifierNameJsonDecorator(name);
		}
		public static IJsonDecorator<Namespace> AsJson(this Namespace @namespace)
		{
			return new NamespaceJsonDecorator(@namespace);
		}
		public static IJsonDecorator<TypeIdentifier> AsJson(this TypeIdentifier identifier)
		{
			return new TypeIdentifierJsonDecorator(identifier);
		}
		public static IJsonDecorator<Error> AsJson(this Error error)
		{
			return new ErrorJsonDecorator(error);
		}
		public static IJsonDecorator<PropertyIdentifier> AsJson(this PropertyIdentifier propertyIdentifier)
		{
			return new PropertyIdentifierJsonDecorator(propertyIdentifier);
		}
		public static IJsonDecorator<Template> AsJson(this Template template)
		{
			return new TemplateJsonDecorator(template);
		}
	}
}
