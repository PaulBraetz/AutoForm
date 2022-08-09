using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoForm.Generate
{
	[Generator]
	public sealed partial class Generator : ISourceGenerator
	{
		public void Execute(GeneratorExecutionContext context)
		{
			var source = String.Empty;
			try
			{
				source = GetControls(context.Compilation) ?? String.Empty;
            }
            catch(Exception ex)
            {
				source = GetError(ex);
				//throw;
            }
            finally
			{
				context.AddSource($"Controls.g", source);
			}
		}

		private String GetError(Exception exception)
        {
			var model = new Source.ErrorModel(exception.Message);
			return Source.GetError(model);
        }

		private String GetControls(Compilation compilation)
		{
			var models = GetModelModels(compilation);
			var modelControlPairs = GetControlModels(compilation);

			return Source.GetControls(models, modelControlPairs);
		}

		private IEnumerable<Source.ModelModel> GetModelModels(Compilation compilation)
		{
			var syntaxTrees = compilation.SyntaxTrees;
			foreach (var syntaxTree in syntaxTrees)
			{
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var modelModels = GetModelDeclarations(syntaxTree).Select(d => GetModelModel(d, semanticModel));
				foreach (var modelModel in modelModels)
				{
					yield return modelModel;
				}
			}
		}
		private IEnumerable<Source.ControlModel> GetControlModels(Compilation compilation)
		{
			var syntaxTrees = compilation.SyntaxTrees;
			foreach (var syntaxTree in syntaxTrees)
			{
				var semanticModel = compilation.GetSemanticModel(syntaxTree);
				var controlModels = GetControlDeclarations(syntaxTree).Select(d => GetControlModel(d, semanticModel));
				foreach (var controlModel in controlModels)
				{
					yield return controlModel;
				}
			}
		}

		private Source.ControlModel GetControlModel(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
		{
			String controlType = GetFullTypeName(classDeclaration, semanticModel);
			String modelType = GetTargetModel(classDeclaration, semanticModel);

			return new Source.ControlModel(controlType, modelType);
		}

		private Source.ModelModel GetModelModel(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
		{
			IEnumerable<Source.PropertyModel> properties = classDeclaration
															.DescendantNodes()
															.OfType<PropertyDeclarationSyntax>()
															.Select(d => GetPropertyModel(d, semanticModel));
			String modelType = GetFullTypeName(classDeclaration, semanticModel);

			return new Source.ModelModel(modelType, properties);
		}

		private Source.PropertyModel GetPropertyModel(PropertyDeclarationSyntax propertyDeclaration, SemanticModel semanticModel)
		{
			String propertyIdentifier = propertyDeclaration.Identifier.ToString();
			String propertyType = GetFullTypeName(propertyDeclaration, semanticModel);

			return new Source.PropertyModel(propertyIdentifier, propertyType);
		}

		private String GetTargetModel(ClassDeclarationSyntax declaration, SemanticModel semanticModel)
		{
			var modelType = declaration.AttributeLists
				.SelectMany(als => als.Attributes)
				.Single(a => a.Name.ToString() == "AutoControl")?
				.ArgumentList
				.Arguments
				.Single()
				.DescendantNodes()
				.OfType<TypeOfExpressionSyntax>()
				.Single()
				.Type;

			var fullModelType = GetFullTypeName(modelType, semanticModel);

			return fullModelType;
		}

		private IEnumerable<ClassDeclarationSyntax> GetModelDeclarations(SyntaxTree syntaxTree)
		{
			if (syntaxTree.TryGetRoot(out SyntaxNode root))
			{
				return root.DescendantNodes()
					.OfType<ClassDeclarationSyntax>()
					.Where(d => d.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == "AutoControlModel")));
			}
			return Array.Empty<ClassDeclarationSyntax>();
		}
		private IEnumerable<ClassDeclarationSyntax> GetControlDeclarations(SyntaxTree syntaxTree)
		{
			if (syntaxTree.TryGetRoot(out SyntaxNode root))
			{
				return root.DescendantNodes()
					.OfType<ClassDeclarationSyntax>()
					.Where(d => d.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == "AutoControl")));
			}
			return Array.Empty<ClassDeclarationSyntax>();
		}

		private String GetFullTypeName(TypeSyntax type, SemanticModel semanticModel)
		{
			var symbol = semanticModel.GetTypeInfo(type).Type;
			return GetFullTypeName(symbol);
		}
		private String GetFullTypeName(PropertyDeclarationSyntax property, SemanticModel semanticModel)
		{
			var symbol = semanticModel.GetDeclaredSymbol(property).Type;
			return GetFullTypeName(symbol);
		}
		private String GetFullTypeName(BaseTypeDeclarationSyntax declaration, SemanticModel semanticModel)
		{
			var symbol = semanticModel.GetDeclaredSymbol(declaration);
			return GetFullTypeName(symbol);
		}

		private String GetFullTypeName(ITypeSymbol symbol)
		{
			var identifier = GetTypeName(symbol);
			var containingNamespace = GetNamespace(symbol);
			return containingNamespace != null ? $"{containingNamespace}.{identifier}" : identifier;
		}
		private String GetTypeName(ITypeSymbol symbol)
		{
			var builder = new StringBuilder();
			Boolean flag = false;
			if (symbol is IArrayTypeSymbol arraySymbol)
			{
				flag = true;
				symbol = arraySymbol.ElementType;
			}

			builder.Append(symbol.Name);
			if (symbol is INamedTypeSymbol namedSymbol && namedSymbol.TypeArguments.Any())
			{
				String[] values = namedSymbol.TypeArguments.Select(GetFullTypeName).ToArray();
				_ = builder.Append($"<{String.Join(", ", values)}>");
			}

			if (flag)
			{
				builder.Append("[]");
			}

			return builder.ToString();
		}
		private String GetNamespace(ISymbol symbol)
		{
			return getNamespace(symbol.ContainingSymbol);
			String getNamespace(ISymbol containingSymbol)
			{
				String symbolName = containingSymbol?.Name;
				if (String.IsNullOrEmpty(symbolName))
				{
					return null;
				}
				if (containingSymbol is INamespaceOrTypeSymbol typeOrNamespaceSymbol)
				{
					String parentNamespace = getNamespace(typeOrNamespaceSymbol.ContainingNamespace);
					return parentNamespace != null ? $"{parentNamespace}.{symbolName}" : symbolName;
				}
				return getNamespace(containingSymbol.ContainingSymbol);
			}
		}

		public void Initialize(GeneratorInitializationContext context)
		{

		}
	}
}
