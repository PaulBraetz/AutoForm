using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace AutoForm.Analysis
{
	internal interface IModelExtractorData
	{
		IEnumerable<BaseTypeDeclarationSyntax> FallbackControls { get; }
		IEnumerable<BaseTypeDeclarationSyntax> FallbackTemplates { get; }
		IEnumerable<BaseTypeDeclarationSyntax> Models { get; }
		IEnumerable<BaseTypeDeclarationSyntax> UseControls { get; }
		IEnumerable<BaseTypeDeclarationSyntax> UseTemplates { get; }
	}
}