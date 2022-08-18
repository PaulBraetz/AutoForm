using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis.Models
{
	internal readonly struct ModelExtractor
	{
		private readonly Compilation _compilation;

		public ModelExtractor(Compilation compilation)
		{
			_compilation = compilation;
		}

		public Error GetErrorModel(Exception ex)
		{
			var error = Error.Create();
			IEnumerable<Exception> exceptions = Flatten(ex);

			foreach (Exception exception in exceptions)
			{
				error = error.Append(exception);
			}

			return error;
		}

		public ModelSpace ExtractModelSpace()
		{
			IEnumerable<Model> models = GetModels();
			IEnumerable<FallbackControl> controls = GetControls();
			IEnumerable<FallbackTemplate> templates = GetTemplates();

			var modelSpace = ModelSpace.Create()
				.AppendRange(models)
				.AppendRange(controls)
				.AppendRange(templates);

			return modelSpace;
		}

		#region Error Methods
		private IEnumerable<Exception> Flatten(Exception exception)
		{
			return exception is AggregateException aggregateException
				? aggregateException.InnerExceptions.SelectMany(Flatten)
				: (new[] { exception });
		}
		#endregion
		#region Modelspace Methods
		private IEnumerable<Model> GetModels()
		{
			IEnumerable<BaseTypeDeclarationSyntax> modelDeclarations = GetModelDeclarations();
			IEnumerable<Model> models = modelDeclarations.Select(GetModel);

			return models;
		}
		private IEnumerable<FallbackControl> GetControls()
		{
			IEnumerable<BaseTypeDeclarationSyntax> controlDeclarations = GetControlDeclarations();
			IEnumerable<FallbackControl> controls = controlDeclarations.Select(GetControl);

			return controls;
		}
		private IEnumerable<FallbackTemplate> GetTemplates()
		{
			IEnumerable<BaseTypeDeclarationSyntax> templateDeclarations = GetTemplateDeclarations();
			IEnumerable<FallbackTemplate> templates = templateDeclarations.Select(GetTemplate);

			return templates;
		}

		private IEnumerable<BaseTypeDeclarationSyntax> GetModelDeclarations()
		{
			IEnumerable<BaseTypeDeclarationSyntax> typeDeclarations = GetTypeDeclarations();
			foreach (BaseTypeDeclarationSyntax typeDeclaration in typeDeclarations)
			{
				if (TryGetAttributes(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.ModelAttribute, out var _))
				{
					yield return typeDeclaration;
				}
			}
		}
		private Model GetModel(BaseTypeDeclarationSyntax typeDeclaration)
		{
			IEnumerable<Property> properties = GetProperties(typeDeclaration);
			TypeIdentifier identifier = GetTypeIdentifier(typeDeclaration);
			PropertyIdentifier attributesProvider = GetAttributesProvider(typeDeclaration);
			TypeIdentifier control = GetControlIdentifier(typeDeclaration);
			TypeIdentifier template = GetTemplateIdentifier(typeDeclaration);

			Model model = Model.Create(identifier, control, template, attributesProvider)
				.AppendRange(properties);

			return model;
		}
		private TypeIdentifier GetControlIdentifier(BaseTypeDeclarationSyntax typeDeclaration)
		{
			TypeIdentifier templateIdentifier = GetTypeArgumentOrDefault(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.ControlAttribute);

			return templateIdentifier;
		}
		private TypeIdentifier GetTemplateIdentifier(BaseTypeDeclarationSyntax typeDeclaration)
		{
			TypeIdentifier templateIdentifier = GetTypeArgumentOrDefault(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.TemplateAttribute);

			return templateIdentifier;
		}
		private PropertyIdentifier GetAttributesProvider(BaseTypeDeclarationSyntax typeDeclaration)
		{
			IEnumerable<PropertyDeclarationSyntax> propertyDeclarations = typeDeclaration.ChildNodes()
				.OfType<PropertyDeclarationSyntax>();

			foreach (PropertyDeclarationSyntax propertyDeclaration in propertyDeclarations)
			{
				if (TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.AttributesProviderAttribute, out var _))
				{
					return PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString());
				}
			}

			return default;
		}
		private Property GetProperty(PropertyDeclarationSyntax propertyDeclaration)
		{
			TypeIdentifier control = GetTypeArgumentOrDefault(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.ControlAttribute);

			TypeIdentifier template = GetTypeArgumentOrDefault(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.TemplateAttribute);

			var identifier = PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString());

			TypeIdentifier type = GetTypeIdentifier(propertyDeclaration);

			Int32 order = GetOrderArgumentOrDefault(propertyDeclaration);

			var property = Property.Create(identifier, type, control, template, order);

			return property;
		}
		private IEnumerable<Property> GetProperties(BaseTypeDeclarationSyntax typeDeclaration)
		{
			IEnumerable<PropertyDeclarationSyntax> propertyDeclarations = GetPropertyDeclarations(typeDeclaration);
			IEnumerable<Property> properties = propertyDeclarations.Select(GetProperty);

			return properties;
		}
		private IEnumerable<PropertyDeclarationSyntax> GetPropertyDeclarations(BaseTypeDeclarationSyntax typeDeclaration)
		{
			IEnumerable<PropertyDeclarationSyntax> propertyDeclarations = typeDeclaration.ChildNodes().OfType<PropertyDeclarationSyntax>();

			foreach (var propertyDeclaration in propertyDeclarations)
			{
				if (!TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.ExcludeAttribute, out var _) &&
					!TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.AttributesProviderAttribute, out var _))
				{
					yield return propertyDeclaration;
				}
			}
		}
		private FallbackControl GetControl(BaseTypeDeclarationSyntax typeDeclaration)
		{
			TypeIdentifier identifier = GetTypeIdentifier(typeDeclaration);

			IEnumerable<TypeIdentifier> modelTypes = GetTypeArguments(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.FallbackControlAttribute);

			var template = FallbackControl.Create(identifier)
				.AppendRange(modelTypes);

			return template;
		}
		private IEnumerable<BaseTypeDeclarationSyntax> GetControlDeclarations()
		{
			var declarations = GetTypeDeclarations();

			foreach (var declaration in declarations)
			{
				if (TryGetAttributes(declaration.AttributeLists, declaration, TypeIdentifier.FallbackControlAttribute, out var _))
				{
					yield return declaration;
				}
			}
		}
		private FallbackTemplate GetTemplate(BaseTypeDeclarationSyntax typeDeclaration)
		{
			TypeIdentifier identifier = GetTypeIdentifier(typeDeclaration);

			IEnumerable<TypeIdentifier> modelTypes = GetTypeArguments(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.FallbackTemplateAttribute);

			var template = FallbackTemplate.Create(identifier).AppendRange(modelTypes);

			return template;
		}
		private IEnumerable<BaseTypeDeclarationSyntax> GetTemplateDeclarations()
		{
			var declarations = GetTypeDeclarations();

			foreach (var declaration in declarations)
			{
				if (TryGetAttributes(declaration.AttributeLists, declaration, TypeIdentifier.FallbackTemplateAttribute, out var _))
				{
					yield return declaration;
				}
			}
		}
		private IEnumerable<BaseTypeDeclarationSyntax> GetTypeDeclarations()
		{
			var declarations = _compilation.SyntaxTrees.SelectMany(t => t.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>());
			return declarations;
		}

		private Int32 GetOrderArgumentOrDefault(PropertyDeclarationSyntax propertyDeclaration)
		{
			TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.OrderAttribute, out IEnumerable<AttributeSyntax> orderAttributes);
			String orderString = orderAttributes.SingleOrDefault()?.DescendantNodes().OfType<AttributeArgumentSyntax>().Single().ToString() ?? "0";
			Int32 order = 0;

			switch (orderString)
			{
				case "int.MaxValue":
				case "Int32.MaxValue":
					order = Int32.MaxValue;
					break;
				case "int.MinValue":
				case "Int32.MinValue":
					order = Int32.MaxValue;
					break;
				default:
					order = Int32.Parse(orderString);
					break;
			}

			return order;
		}

		private TypeIdentifier GetTypeArgumentOrDefault(SyntaxList<AttributeListSyntax> attributeLists, SyntaxNode node, TypeIdentifier attributeIdentifier)
		{
			return GetTypeArguments(attributeLists, node, attributeIdentifier).SingleOrDefault();
		}

		private IEnumerable<TypeIdentifier> GetTypeArguments(SyntaxList<AttributeListSyntax> attributeLists, SyntaxNode node, TypeIdentifier attributeIdentifier)
		{
			if (TryGetAttributes(attributeLists, node, attributeIdentifier, out IEnumerable<AttributeSyntax> attributes))
			{
				IEnumerable<TypeSyntax> modelTypeSyntaxes = attributes.SelectMany(a => a.DescendantNodes()).OfType<TypeOfExpressionSyntax>().Select(e => e.Type);
				IEnumerable<TypeIdentifier> arguments = modelTypeSyntaxes.Select(GetTypeIdentifier);

				return arguments;
			}
			return Array.Empty<TypeIdentifier>();
		}

		private Boolean TryGetAttributes(SyntaxList<AttributeListSyntax> attributeLists, SyntaxNode node, TypeIdentifier attributeIdentifier, out IEnumerable<AttributeSyntax> attributes)
		{
			IEnumerable<Namespace> availableUsings = GetAvailableUsings(node);
			Boolean usingAutoForm = availableUsings.Contains(Namespace.Attributes);

			attributes = attributeLists.SelectMany(al => al.Attributes).Where(a => equals(a));

			return attributes.Any();

			Boolean equals(AttributeSyntax attributeSyntax)
			{
				return attributeSyntax.Name.ToString() == attributeIdentifier.ToEscapedString() ||
					usingAutoForm && attributeSyntax.Name.ToString() == attributeIdentifier.Name.ToEscapedString();

			}
		}
		private IEnumerable<Namespace> GetAvailableUsings(SyntaxNode node)
		{
			var result = new List<Namespace>();

			while (node.Parent != null)
			{
				IEnumerable<UsingDirectiveSyntax> namespaces = node.Parent.ChildNodes().OfType<UsingDirectiveSyntax>();

				foreach (UsingDirectiveSyntax @namespace in namespaces)
				{
					Namespace item = Namespace.Create()
						.AppendRange(@namespace.Name.ToString().Split('.'));

					result.Add(item);
				}

				node = node.Parent;
			}

			return result;
		}
		#region Type Methods
		private TypeIdentifier GetTypeIdentifier(TypeSyntax type)
		{
			SemanticModel semanticModel = _compilation.GetSemanticModel(type.SyntaxTree);
			ITypeSymbol symbol = semanticModel.GetDeclaredSymbol(type) as ITypeSymbol ?? semanticModel.GetTypeInfo(type).Type;

			TypeIdentifier identifier = GetTypeIdentifier(symbol);

			return identifier;
		}
		private TypeIdentifier GetTypeIdentifier(PropertyDeclarationSyntax property)
		{
			SemanticModel semanticModel = _compilation.GetSemanticModel(property.SyntaxTree);
			ITypeSymbol symbol = semanticModel.GetDeclaredSymbol(property).Type;

			TypeIdentifier identifier = GetTypeIdentifier(symbol);

			return identifier;
		}
		private TypeIdentifier GetTypeIdentifier(BaseTypeDeclarationSyntax declaration)
		{
			SemanticModel semanticModel = _compilation.GetSemanticModel(declaration.SyntaxTree);
			INamedTypeSymbol symbol = semanticModel.GetDeclaredSymbol(declaration);

			TypeIdentifier identifier = GetTypeIdentifier(symbol);

			return identifier;
		}

		private TypeIdentifier GetTypeIdentifier(ITypeSymbol symbol)
		{
			TypeIdentifierName identifier = GetTypeIdentifierName(symbol);
			Namespace @namespace = GetNamespace(symbol);

			return TypeIdentifier.Create(identifier, @namespace);
		}
		private Namespace GetNamespace(ISymbol symbol)
		{
			var result = Namespace.Create();

			while (symbol != null && symbol.Name != String.Empty)
			{
				if (symbol is INamespaceSymbol)
				{
					result = result.Prepend(symbol.Name);
				}

				symbol = symbol.ContainingNamespace;
			}

			return result;
		}
		private TypeIdentifierName GetTypeIdentifierName(ITypeSymbol symbol)
		{
			var result = TypeIdentifierName.Create();

			if (symbol.ContainingType != null)
			{
				TypeIdentifierName containingType = GetTypeIdentifierName(symbol.ContainingType);
				result = result.AppendTypePart(containingType);
			}

			Boolean flag = false;
			if (symbol is IArrayTypeSymbol arraySymbol)
			{
				flag = true;
				symbol = arraySymbol.ElementType;
			}

			result = result.AppendNamePart(symbol.Name);

			if (symbol is INamedTypeSymbol namedSymbol && namedSymbol.TypeArguments.Any())
			{
				IEnumerable<TypeIdentifier> arguments = namedSymbol.TypeArguments.Select(GetTypeIdentifier);
				result = result.AppendGenericPart(arguments);
			}

			if (flag)
			{
				result = result.AppendArrayPart();
			}

			return result;
		}
		#endregion
		#endregion
	}
}
