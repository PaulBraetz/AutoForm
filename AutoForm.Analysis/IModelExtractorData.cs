using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace AutoForm.Analysis
{
	internal interface IModelExtractorData
	{
		IEnumerable<BaseTypeDeclarationSyntax> Controls { get; }
		IEnumerable<BaseTypeDeclarationSyntax> DefaultTemplates { get; }
		IEnumerable<BaseTypeDeclarationSyntax> DefaultModels { get; }
	}
}