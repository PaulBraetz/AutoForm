using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Json.Analysis
{
	internal static class GeneratedIdentifiers
	{
		public static readonly INamespace Namespace = RhoMicro.CodeAnalysis.Namespace.Create().Append("AutoForm").Append("Generated");
		public static readonly ITypeIdentifier GeneratedControls = TypeIdentifier.Create(TypeIdentifierName.Create().AppendNamePart("Controls"), Namespace);
	}
}
