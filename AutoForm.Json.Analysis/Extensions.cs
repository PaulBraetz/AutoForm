using AutoForm.Analysis.Models;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Json.Analysis
{
	internal static class Extensions
	{
		public static IdentifierPartJsonDecorator AsJson(this IdentifierPart part)
		{
			return new IdentifierPartJsonDecorator(part);
		}
		public static TypeIdentifierNameJsonDecorator AsJson(this TypeIdentifierName name)
		{
			return new TypeIdentifierNameJsonDecorator(name);
		}
		public static NamespaceJsonDecorator AsJson(this RhoMicro.CodeAnalysis.Namespace @namespace)
		{
			return new NamespaceJsonDecorator(@namespace);
		}
		public static TypeIdentifierJsonDecorator AsJson(this TypeIdentifier identifier)
		{
			return new TypeIdentifierJsonDecorator(identifier);
		}
	}
}
