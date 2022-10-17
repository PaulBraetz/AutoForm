using AutoForm.Analysis;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoForm.Json.Analysis
{
	internal static class Extensions
	{
		public static String ToJson(this IdentifierPart part)
		{
			return Json.Value(part.ToString());
		}
		public static String AsJson(this TypeIdentifierName name)
		{
			return Analysis.Json.Value(name.ToString());
		}
		public static String AsJson(this Namespace @namespace)
		{
			return Analysis.Json.Value(@namespace.ToString());
		}
		public static String AsJson(this TypeIdentifier identifier)
		{
			return Analysis.Json.Value(identifier.ToString());
		}
		public static String AsJson(this Error error)
		{
			return Json.Object(Json.KeyValuePair(nameof(error.Exceptions), error.Exceptions.Select(m => m.Message)));
		}
		public static String AsJson(this PropertyIdentifier propertyIdentifier)
		{
			return Json.Value(propertyIdentifier.ToString());
		}
		public static String AsJson(this Template template)
		{
			return Json.Object(Json.KeyValuePair(nameof(template.Name), template.Name.AsJson()),
								Json.KeyValuePair(nameof(template.Models), template.Models.Select(m=>m.AsJson())));
		}
	}
}
