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
                var modelModels = GetModelDeclarations(syntaxTree).Select(d => GetModelModel(d, compilation));
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
                var controlModels = GetControlDeclarations(syntaxTree).Select(d => GetControlModel(d, compilation));
                foreach (var controlModel in controlModels)
                {
                    yield return controlModel;
                }
            }
        }

        private Source.ControlModel GetControlModel(ClassDeclarationSyntax classDeclaration, Compilation compilation)
        {
            String controlType = GetFullTypeName(classDeclaration, compilation);
            IEnumerable< String> modelTypes = GetTargetModelTypes(classDeclaration, compilation);

            return new Source.ControlModel(controlType, modelTypes);
        }

        private Source.ModelModel GetModelModel(ClassDeclarationSyntax classDeclaration, Compilation compilation)
        {
            IEnumerable<PropertyDeclarationSyntax> properties = classDeclaration
                                                                    .ChildNodes()
                                                                    .OfType<PropertyDeclarationSyntax>();

            var attributesProvider = properties.SingleOrDefault(d => d.AttributeLists.Any(al => al.Attributes.Any(a => a.Name.ToString() == "AutoControlAttributesProvider")));

            IEnumerable<Source.PropertyModel> propertyModels = properties
                                                                .Where(d => !d.AttributeLists
                                                                    .Any(al => al.Attributes
                                                                        .Any(a => a.Name.ToString() == "AutoControlAttributesProvider" ||
                                                                                  a.Name.ToString() == "AutoControlModelExclude")))
                                                                .OrderBy(d=>Int32.Parse(d.AttributeLists
                                                                    .SelectMany(l => l.Attributes)
                                                                    .SingleOrDefault(a => a.Name.ToString() == "AutoControlPropertyPosition")?
                                                                    .ArgumentList.Arguments.Single().ToString() ?? "0"))
                                                                .Select(d => GetPropertyModel(d, compilation));

            String modelType = GetFullTypeName(classDeclaration, compilation);

            return new Source.ModelModel(modelType, attributesProvider?.Identifier.ToString(), propertyModels);
        }

        private Source.PropertyModel GetPropertyModel(PropertyDeclarationSyntax propertyDeclaration, Compilation compilation)
        {
            String propertyIdentifier = propertyDeclaration.Identifier.ToString();
            String propertyType = GetFullTypeName(propertyDeclaration, compilation);
            String propertyControlType = GetPropertyControlType(propertyDeclaration, compilation);

            return new Source.PropertyModel(propertyType, propertyIdentifier, propertyControlType);
        }
        private string GetPropertyControlType(PropertyDeclarationSyntax propertyDeclaration, Compilation compilation)
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

                return GetFullTypeName(typeSyntax, compilation);
            }

            return null;
        }
        private IEnumerable<String> GetTargetModelTypes(ClassDeclarationSyntax declaration, Compilation compilation)
        {
            var types = declaration.AttributeLists
                .SelectMany(als => als.Attributes)
                .Where(a => a.Name.ToString() == "AutoControl")
                .Select(a=>a.ArgumentList
                    .Arguments
                    .Single()
                    .DescendantNodes()
                    .OfType<TypeOfExpressionSyntax>()
                    .Single()
                    .Type)
                .Select(s=> GetFullTypeName(s, compilation));

            return types;
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

        private String GetFullTypeName(TypeSyntax type, Compilation compilation)
        {
            var semanticModel = compilation.GetSemanticModel(type.SyntaxTree);
            var symbol = semanticModel.GetDeclaredSymbol(type) as ITypeSymbol ?? semanticModel.GetTypeInfo(type).Type;
            return GetFullTypeName(symbol);
        }
        private String GetFullTypeName(PropertyDeclarationSyntax property, Compilation compilation)
        {
            var semanticModel = compilation.GetSemanticModel(property.SyntaxTree);
            var symbol = semanticModel.GetDeclaredSymbol(property).Type;
            return GetFullTypeName(symbol);
        }
        private String GetFullTypeName(BaseTypeDeclarationSyntax declaration, Compilation compilation)
        {
            var semanticModel = compilation.GetSemanticModel(declaration.SyntaxTree);
            var symbol = semanticModel.GetDeclaredSymbol(declaration);
            return GetFullTypeName(symbol);
        }

        private String GetFullTypeName(ITypeSymbol symbol)
        {
            var identifier = GetTypeName(symbol);
            var containingNamespace = GetNamespace(symbol.ContainingSymbol);
            var containingNamespaceString = containingNamespace != String.Empty ? $"{containingNamespace}." : String.Empty;
            return $"{containingNamespaceString}{identifier}";
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
