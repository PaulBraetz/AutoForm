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
			var exceptions = Flatten(ex);

			foreach (var exception in exceptions)
			{
				error = error.With(exception);
			}

			return error;
		}

		public ModelSpace ExtractModelSpace()
		{
			var models = GetModels();
			var controls = GetControls();
			var templates = GetTemplates();

			var modelSpace = ModelSpace.Create()
				.WithModels(models)
				.WithFallbackControls(controls)
				.WithTemplates(templates);

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
			var modelDeclarations = GetModelDeclarations();
			var models = modelDeclarations.Select(GetModel);

			return models;
		}
		private IEnumerable<Control> GetControls()
		{
			var controlDeclarations = GetControlDeclarations();
			var controls = controlDeclarations.Select(GetControl);

			return controls;
		}
		private IEnumerable<Template> GetTemplates()
		{
			var templateDeclarations = GetTemplateDeclarations();
			var templates = templateDeclarations.Select(GetTemplate);

			return templates;
		}

		private IEnumerable<BaseTypeDeclarationSyntax> GetModelDeclarations()
		{
			var typeDeclarations = GetTypeDeclarations();
			foreach (var typeDeclaration in typeDeclarations)
			{
				if (TryGetAttributes(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.ModelAttribute, out var _))
				{
					yield return typeDeclaration;
				}
			}
		}
		private Model GetModel(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var properties = GetProperties(typeDeclaration);
			var identifier = GetTypeIdentifier(typeDeclaration);
			var attributesProvider = GetAttributesProvider(typeDeclaration);
			var control = GetControlIdentifier(typeDeclaration);
			var template = GetTemplateIdentifier(typeDeclaration);

			var model = Model.Create(identifier, control, template, attributesProvider)
				.WithRange(properties);

			return model;
		}
		private TypeIdentifier GetControlIdentifier(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var templateIdentifier = GetTypeArgumentOrDefault(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.ControlAttribute);

			return templateIdentifier;
		}
		private TypeIdentifier GetTemplateIdentifier(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var templateIdentifier = GetTypeArgumentOrDefault(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.TemplateAttribute);

			return templateIdentifier;
		}
		private PropertyIdentifier GetAttributesProvider(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var propertyDeclarations = typeDeclaration.ChildNodes()
				.OfType<PropertyDeclarationSyntax>();

			foreach (var propertyDeclaration in propertyDeclarations)
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
			var control = GetTypeArgumentOrDefault(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.ControlAttribute);

			var template = GetTypeArgumentOrDefault(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.TemplateAttribute);

			var identifier = PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString());

			var type = GetTypeIdentifier(propertyDeclaration);

			var order = GetOrderArgumentOrDefault(propertyDeclaration);

			var property = Property.Create(identifier, type, control, template, order);

			return property;
		}
		private IEnumerable<Property> GetProperties(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var propertyDeclarations = GetPropertyDeclarations(typeDeclaration);
			var properties = propertyDeclarations.Select(GetProperty);

			return properties;
		}
		private IEnumerable<PropertyDeclarationSyntax> GetPropertyDeclarations(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var propertyDeclarations = typeDeclaration.ChildNodes().OfType<PropertyDeclarationSyntax>();

			foreach (var propertyDeclaration in propertyDeclarations)
			{
				if (!TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.ExcludeAttribute, out var _) &&
					!TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.AttributesProviderAttribute, out var _))
				{
					yield return propertyDeclaration;
				}
			}
		}
		private Control GetControl(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var identifier = GetTypeIdentifier(typeDeclaration);

			var modelTypes = GetTypeArguments(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.FallbackControlAttribute);

			var template = Control.Create(identifier)
				.WithRange(modelTypes);

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
		private Template GetTemplate(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var identifier = GetTypeIdentifier(typeDeclaration);

			var modelTypes = GetTypeArguments(typeDeclaration.AttributeLists, typeDeclaration, TypeIdentifier.FallbackTemplateAttribute);

			var template = Template.Create(identifier).WithRange(modelTypes);

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
			TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, TypeIdentifier.OrderAttribute, out var orderAttributes);
			var orderString = orderAttributes.SingleOrDefault()?.DescendantNodes().OfType<AttributeArgumentSyntax>().Single().ToString() ?? "0";
			var order = 0;

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
			if (TryGetAttributes(attributeLists, node, attributeIdentifier, out var attributes))
			{
				var modelTypeSyntaxes = attributes.SelectMany(a => a.DescendantNodes()).OfType<TypeOfExpressionSyntax>().Select(e => e.Type);
				var arguments = modelTypeSyntaxes.Select(GetTypeIdentifier);

				return arguments;
			}

			return Array.Empty<TypeIdentifier>();
		}

		private Boolean TryGetAttributes(SyntaxList<AttributeListSyntax> attributeLists, SyntaxNode node, TypeIdentifier attributeIdentifier, out IEnumerable<AttributeSyntax> attributes)
		{
			var availableUsings = GetAvailableUsings(node);
			var usingAutoForm = availableUsings.Contains(Namespace.Attributes);

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
				var namespaces = node.Parent.ChildNodes().OfType<UsingDirectiveSyntax>();

				foreach (var @namespace in namespaces)
				{
					var item = Namespace.Create()
						.WithRange(@namespace.Name.ToString().Split('.'));

					result.Add(item);
				}

				node = node.Parent;
			}

			return result;
		}
		#region Type Methods
		private TypeIdentifier GetTypeIdentifier(TypeSyntax type)
		{
			var semanticModel = _compilation.GetSemanticModel(type.SyntaxTree);
			var symbol = semanticModel.GetDeclaredSymbol(type) as ITypeSymbol ?? semanticModel.GetTypeInfo(type).Type;

			var identifier = GetTypeIdentifier(symbol);

			return identifier;
		}
		private TypeIdentifier GetTypeIdentifier(PropertyDeclarationSyntax property)
		{
			var semanticModel = _compilation.GetSemanticModel(property.SyntaxTree);
			var symbol = semanticModel.GetDeclaredSymbol(property).Type;

			var identifier = GetTypeIdentifier(symbol);

			return identifier;
		}
		private TypeIdentifier GetTypeIdentifier(BaseTypeDeclarationSyntax declaration)
		{
			var semanticModel = _compilation.GetSemanticModel(declaration.SyntaxTree);
			var symbol = semanticModel.GetDeclaredSymbol(declaration);

			var identifier = GetTypeIdentifier(symbol);

			return identifier;
		}

		private TypeIdentifier GetTypeIdentifier(ITypeSymbol symbol)
		{
			var identifier = GetTypeIdentifierName(symbol);
			var @namespace = GetNamespace(symbol);

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
				var containingType = GetTypeIdentifierName(symbol.ContainingType);
				result = result.WithTypePart(containingType);
			}

			var flag = false;
			if (symbol is IArrayTypeSymbol arraySymbol)
			{
				flag = true;
				symbol = arraySymbol.ElementType;
			}

			result = result.WithNamePart(symbol.Name);

			if (symbol is INamedTypeSymbol namedSymbol && namedSymbol.TypeArguments.Any())
			{
				var arguments = new TypeIdentifier[namedSymbol.TypeArguments.Length];

				for (var i = 0; i < arguments.Length; i++)
				{
					var typeArgument = namedSymbol.TypeArguments[i];
					TypeIdentifier argument = default;
					if (SymbolEqualityComparer.Default.Equals(typeArgument.ContainingType, namedSymbol))
					{
						argument = TypeIdentifier.Create(TypeIdentifierName.Create().WithNamePart(typeArgument.ToString()), Namespace.Create());
					}
					else
					{
						argument = GetTypeIdentifier(typeArgument);
					}

					arguments[i] = argument;
				}

				result = result.WithGenericPart(arguments);
			}

			if (flag)
			{
				result = result.WithArrayPart();
			}

			return result;
		}
		#endregion
		#endregion
	}
}
