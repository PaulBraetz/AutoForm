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
            catch (Exception ex)
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
            var controls = GetControlModels(compilation);

            return Source.GetControls(models, controls);
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
            IEnumerable<PropertyDeclarationSyntax> properties = classDeclaration
                                                                    .ChildNodes()
                                                                    .OfType<PropertyDeclarationSyntax>();

            var attributesProvider = properties.SingleOrDefault(d => d.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == "AutoControlAttributesProvider")));

            IEnumerable<Source.PropertyModel> propertyModels = properties
                                                                .Where(d => !d.AttributeLists.Any(al => al.Attributes
                                                                    .Any(a => a.Name.ToString() == "AutoControlAttributesProvider" ||
                                                                              a.Name.ToString() == "AutoControlModelExclude")))
                                                                .Select(d => GetPropertyModel(d, semanticModel));

            String modelType = GetFullTypeName(classDeclaration, semanticModel);

            return new Source.ModelModel(modelType, attributesProvider?.Identifier.ToString(), propertyModels);
        }

        private Source.PropertyModel GetPropertyModel(PropertyDeclarationSyntax propertyDeclaration, SemanticModel semanticModel)
        {
            String propertyIdentifier = propertyDeclaration.Identifier.ToString();
            String propertyType = GetFullTypeName(propertyDeclaration, semanticModel);
            String propertyControlType = GetPropertyControlType(propertyDeclaration, semanticModel);

            return new Source.PropertyModel(propertyType, propertyIdentifier, propertyControlType);
        }
        private string GetPropertyControlType(PropertyDeclarationSyntax propertyDeclaration, SemanticModel semanticModel)
        {
            var attribute = propertyDeclaration.AttributeLists
                .SelectMany(al => al.Attributes)
                .SingleOrDefault(a => a.Name.ToString() == "AutoControlModelProperty");

            if (attribute != null)
            {
                var typeSyntax = attribute.ArgumentList
                                    .Arguments
                                    .Single()
                                    .DescendantNodes()
                                    .OfType<TypeOfExpressionSyntax>()
                                    .Single()
                                    .Type;

                return GetFullTypeName(typeSyntax, semanticModel);
            }

            return null;
        }
        private String GetTargetModel(ClassDeclarationSyntax declaration, SemanticModel semanticModel)
        {
            var typeSyntax = declaration.AttributeLists
                .SelectMany(als => als.Attributes)
                .Single(a => a.Name.ToString() == "AutoControl")
                .ArgumentList
                .Arguments
                .Single()
                .DescendantNodes()
                .OfType<TypeOfExpressionSyntax>()
                .Single()
                .Type;

            return GetFullTypeName(typeSyntax, semanticModel);
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
            var symbol = semanticModel.GetDeclaredSymbol(type) as ITypeSymbol ?? semanticModel.GetTypeInfo(type).Type;
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
            var containingNamespace = GetNamespace(symbol.ContainingSymbol);
            return $"{containingNamespace}.{identifier}";
        }

        private String GetNamespace(ISymbol symbol)
        {
            if(symbol is null || symbol.Name == String.Empty)
            {
                return String.Empty;
            }
            var containingNamespace = GetNamespace(symbol.ContainingSymbol);
            var containingNamespaceValue = containingNamespace != String.Empty ? $"{containingNamespace}." : String.Empty;
            return $"{containingNamespaceValue}{symbol.Name}";
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

        public void Initialize(GeneratorInitializationContext context)
        {

        }
    }
}
