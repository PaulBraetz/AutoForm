using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Models
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
			IEnumerable<Control> controls = GetControls();
			IEnumerable<Template> templates = GetTemplates();

			var modelSpace = ModelSpace.Create(models, controls, templates);

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
		private IEnumerable<Control> GetControls()
		{
			IEnumerable<BaseTypeDeclarationSyntax> controlDeclarations = GetControlDeclarations();
			IEnumerable<Control> controls = controlDeclarations.Select(GetControl);

			return controls;
		}
		private IEnumerable<Template> GetTemplates()
		{
			IEnumerable<BaseTypeDeclarationSyntax> templateDeclarations = GetTemplateDeclarations();
			IEnumerable<Template> templates = templateDeclarations.Select(GetTemplate);

			return templates;
		}

		private IEnumerable<BaseTypeDeclarationSyntax> GetModelDeclarations()
		{
			IEnumerable<BaseTypeDeclarationSyntax> typeDeclarations = GetTypeDeclarations();
			foreach (BaseTypeDeclarationSyntax typeDeclaration in typeDeclarations)
			{
				if (TryGetAttributes(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.AutoControlModelAttribute, out var _))
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

			Model model = Model.Create(identifier, attributesProvider)
				.AppendRange(properties);

			return model;
		}
		private PropertyIdentifier GetAttributesProvider(BaseTypeDeclarationSyntax typeDeclaration)
		{
			IEnumerable<PropertyDeclarationSyntax> propertyDeclarations = typeDeclaration.ChildNodes()
				.OfType<PropertyDeclarationSyntax>();

			foreach (PropertyDeclarationSyntax propertyDeclaration in propertyDeclarations)
			{
				if (TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.AutoControlAttributesProviderAttribute, out var _))
				{
					return PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString());
				}
			}

			return default;
		}
		private Property GetProperty(PropertyDeclarationSyntax propertyDeclaration)
		{
			TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.AutoControlPropertyControlAttribute, out IEnumerable<AttributeSyntax> controlAttributes);
			TypeSyntax controlTypeSyntax = controlAttributes.SingleOrDefault()?.DescendantNodes().OfType<TypeOfExpressionSyntax>().Single().Type;
			TypeIdentifier control = controlTypeSyntax != null ? GetTypeIdentifier(controlTypeSyntax) : default;

			var identifier = PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString());

			TypeIdentifier type = GetTypeIdentifier(propertyDeclaration);

			TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.AutoControlPropertyOrderAttribute, out IEnumerable<AttributeSyntax> orderAttributes);
			String orderString = orderAttributes.SingleOrDefault()?.DescendantNodes().OfType<AttributeArgumentSyntax>().Single().ToString() ?? "0";
			Int32 order = Int32.Parse(orderString);

			var property = Property.Create(identifier, type, control, order);

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
				if (!TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.AutoControlPropertyExcludeAttribute, out var _) &&
					!TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.AutoControlAttributesProviderAttribute, out var _))
				{
					yield return propertyDeclaration;
				}
			}
		}
		private Control GetControl(BaseTypeDeclarationSyntax typeDeclaration)
		{
			TypeIdentifier identifier = GetTypeIdentifier(typeDeclaration);

			TryGetAttributes(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.AutoControlAttribute, out IEnumerable<AttributeSyntax> attributes);
			IEnumerable<TypeSyntax> modelTypeSyntaxes = attributes.SelectMany(a => a.DescendantNodes()).OfType<TypeOfExpressionSyntax>().Select(e => e.Type);
			IEnumerable<TypeIdentifier> modelTypes = modelTypeSyntaxes.Select(GetTypeIdentifier);

			var template = Control.Create(identifier)
				.AppendRange(modelTypes);

			return template;
		}
		private IEnumerable<BaseTypeDeclarationSyntax> GetControlDeclarations()
		{
			var declarations = GetTypeDeclarations();

			foreach (var declaration in declarations)
			{
				if (TryGetAttributes(declaration.AttributeLists, declaration, TypeIdentifier.AutoControlAttribute, out var _))
				{
					yield return declaration;
				}
			}
		}
		private Template GetTemplate(BaseTypeDeclarationSyntax typeDeclaration)
		{
			TypeIdentifier identifier = GetTypeIdentifier(typeDeclaration);

			TryGetAttributes(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.AutoControlTemplateAttribute, out IEnumerable<AttributeSyntax> attributes);
			IEnumerable<TypeSyntax> modelTypeSyntaxes = attributes.SelectMany(a => a.DescendantNodes()).OfType<TypeOfExpressionSyntax>().Select(e => e.Type);
			IEnumerable<TypeIdentifier> modelTypes = modelTypeSyntaxes.Select(GetTypeIdentifier);

			var template = Template.Create(identifier).AppendRange(modelTypes);

			return template;
		}
		private IEnumerable<BaseTypeDeclarationSyntax> GetTemplateDeclarations()
		{
			var declarations = GetTypeDeclarations();

			foreach (var declaration in declarations)
			{
				if (TryGetAttributes(declaration.AttributeLists, declaration, TypeIdentifier.AutoControlTemplateAttribute, out var _))
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
		private Boolean TryGetAttributes(SyntaxList<AttributeListSyntax> attributeLists, SyntaxNode node, TypeIdentifier attributeIdentifier, out IEnumerable<AttributeSyntax> attributes)
		{
			IEnumerable<Namespace> availableUsings = GetAvailableUsings(node);
			Boolean usingAutoForm = availableUsings.Contains(Namespace.Attributes);

			attributes = attributeLists.SelectMany(al => al.Attributes).Where(a => equals(a));

			return attributes.Any();

			Boolean equals(AttributeSyntax attributeSyntax)
			{
				return attributeSyntax.Name.ToString() == attributeIdentifier.ToString() ||
					usingAutoForm && attributeSyntax.Name.ToString() == attributeIdentifier.Name.ToString();

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
