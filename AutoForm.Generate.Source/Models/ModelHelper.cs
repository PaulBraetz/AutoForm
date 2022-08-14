using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoForm.Generate.Models
{
	public readonly struct ModelHelper
	{
		private const string CONTROL = "AutoControl";
		private const string ATTRIBUTES_PROVIDER = "AutoControlAttributesProvider";
		private const String MODEL = "AutoControlModel";
		private const String PROPERTY_CONTROL = "AutoControlPropertyControl";
		private const String PROPERTY_EXCLUDE = "AutoControlPropertyExclude";
		private const String PROPERTY_ORDER = "AutoControlPropertyOrder";
		private const String TEMPLATE = "AutoControlTemplate";

		private readonly Compilation _compilation;

		public ModelHelper(Compilation compilation)
		{
			_compilation = compilation;
		}

		public Error GetError(Exception ex)
		{
			var error = Error.Create();
			IEnumerable<Exception> exceptions = Flatten(ex);

			foreach (Exception exception in exceptions)
			{
				error = error.Append(exception);
			}

			return error;
		}

		public ModelSpace GetModelSpace()
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
				if (TryGetAttributes(typeDeclaration.AttributeLists, typeDeclaration, MODEL, out var _))
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
				if (TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, ATTRIBUTES_PROVIDER, out var _))
				{
					return PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString());
				}
			}

			return default;
		}
		private Property GetProperty(PropertyDeclarationSyntax propertyDeclaration)
		{
			TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, PROPERTY_CONTROL, out IEnumerable<AttributeSyntax> controlAttributes);
			TypeSyntax controlTypeSyntax = controlAttributes.SingleOrDefault()?.DescendantNodes().OfType<TypeOfExpressionSyntax>().Single().Type;
			TypeIdentifier control = controlTypeSyntax != null ? GetTypeIdentifier(controlTypeSyntax) : default;

			var identifier = PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString());

			TypeIdentifier type = GetTypeIdentifier(propertyDeclaration);

			TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, PROPERTY_ORDER, out IEnumerable<AttributeSyntax> orderAttributes);
			String orderString = orderAttributes.SingleOrDefault()?.DescendantNodes().OfType<LiteralExpressionSyntax>().Single().ToString() ?? "0";
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
				if (!TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, PROPERTY_EXCLUDE, out var _) &&
					!TryGetAttributes(propertyDeclaration.AttributeLists, propertyDeclaration, ATTRIBUTES_PROVIDER, out var _))
				{
					yield return propertyDeclaration;
				}
			}
		}
		private Control GetControl(BaseTypeDeclarationSyntax typeDeclaration)
		{
			TypeIdentifier identifier = GetTypeIdentifier(typeDeclaration);

			TryGetAttributes(typeDeclaration.AttributeLists, typeDeclaration, CONTROL, out IEnumerable<AttributeSyntax> attributes);
			TypeSyntax modelTypeSyntax = attributes.Single().DescendantNodes().OfType<TypeOfExpressionSyntax>().Single().Type;
			TypeIdentifier modelType = GetTypeIdentifier(modelTypeSyntax);

			var template = Control.Create(identifier, modelType);

			return template;
		}
		private IEnumerable<BaseTypeDeclarationSyntax> GetControlDeclarations()
		{
			var declarations = GetTypeDeclarations();

			foreach (var declaration in declarations)
			{
				if (TryGetAttributes(declaration.AttributeLists, declaration, CONTROL, out var _))
				{
					yield return declaration;
				}
			}
		}
		private Template GetTemplate(BaseTypeDeclarationSyntax typeDeclaration)
		{
			TypeIdentifier identifier = GetTypeIdentifier(typeDeclaration);

			TryGetAttributes(typeDeclaration.AttributeLists, typeDeclaration, TEMPLATE, out IEnumerable<AttributeSyntax> attributes);
			TypeSyntax modelTypeSyntax = attributes.Single().DescendantNodes().OfType<TypeOfExpressionSyntax>().Single().Type;
			TypeIdentifier modelType = GetTypeIdentifier(modelTypeSyntax);

			var template = Template.Create(identifier, modelType);

			return template;
		}
		private IEnumerable<BaseTypeDeclarationSyntax> GetTemplateDeclarations()
		{
			var declarations = GetTypeDeclarations();

			foreach (var declaration in declarations)
			{
				if (TryGetAttributes(declaration.AttributeLists, declaration, TEMPLATE, out var _))
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
		private Boolean TryGetAttributes(SyntaxList<AttributeListSyntax> attributeLists, SyntaxNode node, String attributeName, out IEnumerable<AttributeSyntax> attributes)
		{
			IEnumerable<Namespace> availableUsings = GetAvailableUsings(node);
			Namespace autoFormNamespace = Namespace.Create().Append("AutoForm").Append("Attributes");
			Boolean usingAutoForm = availableUsings.Contains(autoFormNamespace);
			attributeName = usingAutoForm ? attributeName : TypeIdentifier.Create(TypeIdentifierName.Create().AppendNamePart(attributeName), autoFormNamespace).ToString();

			var t = attributeLists.SelectMany(al => al.Attributes).Select(a => a.Name.ToString());

			attributes = attributeLists.SelectMany(al => al.Attributes).Where(a => a.Name.ToString() == attributeName);

			return attributes.Any();
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
