using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoForm.Analysis
{
	internal interface IModelExtractorData
	{
		IEnumerable<BaseTypeDeclarationSyntax> Controls {
			get;
		}
		IEnumerable<BaseTypeDeclarationSyntax> DefaultTemplates {
			get;
		}
		IEnumerable<BaseTypeDeclarationSyntax> DefaultModels {
			get;
		}
	}
}