using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
	internal readonly struct ModelExtractor
	{
		private readonly Compilation _compilation;
		private readonly IModelExtractorData _data;

		public ModelExtractor(Compilation compilation, IModelExtractorData data)
		{
			_compilation = compilation;
			_data = data;
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

		#region Modelspace Methods
		private IEnumerable<Model> GetModels()
		{
			var models = _data.Models.Select(GetModel);

			return models;
		}
		private IEnumerable<Control> GetControls()
		{
			var controlDeclarations = _data.FallbackControls;
			var controls = controlDeclarations.Select(GetControl);

			return controls;
		}
		private IEnumerable<Template> GetTemplates()
		{
			var templateDeclarations = _data.FallbackTemplates;
			var templates = templateDeclarations.Select(GetTemplate);

			return templates;
		}

		private Model GetModel(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var properties = GetProperties(typeDeclaration);
			var identifier = GetType(typeDeclaration);
			var attributesProvider = GetAttributesProvider(typeDeclaration);
			var control = GetControlIdentifier(typeDeclaration);
			var template = GetTemplateIdentifier(typeDeclaration);

			var model = Model.Create(identifier, control, template, attributesProvider)
				.WithRange(properties);

			return model;
		}
		private ITypeIdentifier GetControlIdentifier(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var semanticModel = GetSemanticModel(typeDeclaration);
			var controlIdentifier = typeDeclaration.AttributeLists
				.OfAttributeClasses(semanticModel, Attributes.UseControl)
				.Select(a => (success: Attributes.Factories.UseControl.TryBuild(a, semanticModel, out var attribute), attribute))
				.SingleOrDefault(t => t.success)
				.attribute?
				.GetTypeParameter("controlType") as ITypeIdentifier;

			return controlIdentifier;
		}
		private ITypeIdentifier GetTemplateIdentifier(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var semanticModel = GetSemanticModel(typeDeclaration);
			var templateIdentifier = typeDeclaration.AttributeLists
				.OfAttributeClasses(semanticModel, Attributes.UseTemplate)
				.Select(a => (success: Attributes.Factories.UseTemplate.TryBuild(a, semanticModel, out var attribute), attribute))
				.SingleOrDefault(t => t.success)
				.attribute?
				.GetTypeParameter("templateType") as ITypeIdentifier;

			return templateIdentifier;
		}
		private PropertyIdentifier GetAttributesProvider(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var semanticModel = GetSemanticModel(typeDeclaration);
			var providerProperty = typeDeclaration.ChildNodes()
				.OfType<PropertyDeclarationSyntax>()
				.Where(p => p.AttributeLists
					.OfAttributeClasses(semanticModel, Attributes.AttributesProvider)
					.Select(a => (success: Attributes.Factories.AttributesProvider.TryBuild(a, semanticModel, out var attribute), attribute))
					.Any(t => t.success))
				.SingleOrDefault();
			var identifier = providerProperty != null ?
				PropertyIdentifier.Create(providerProperty.Identifier.ToString()) :
				default;

			return identifier;
		}
		private Property GetProperty(PropertyDeclarationSyntax propertyDeclaration)
		{
			var semanticModel = GetSemanticModel(propertyDeclaration);
			var modelPropertyAttribute = propertyDeclaration.AttributeLists
				.OfAttributeClasses(semanticModel, Attributes.ModelProperty)
				.Select(a => (success: Attributes.Factories.ModelProperty.TryBuild(a, semanticModel, out var attribute), attribute))
				.Single(t => t.success)
				.attribute;

			var control = modelPropertyAttribute.GetTypeParameter("controlType") as ITypeIdentifier;

			var template = modelPropertyAttribute.GetTypeParameter("templateType") as ITypeIdentifier;

			var identifier = PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString());

			var type = GetType(propertyDeclaration.Type);

			var order = modelPropertyAttribute.Order;

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
			var semanticModel = GetSemanticModel(typeDeclaration);
			var propertyDeclarations = typeDeclaration.ChildNodes()
				.OfType<PropertyDeclarationSyntax>()
				.Where(p => p.AttributeLists
					.OfAttributeClasses(semanticModel, Attributes.ModelProperty)
					.Select(a => (success: Attributes.Factories.ModelProperty.TryBuild(a, semanticModel, out var attribute), attribute))
					.SingleOrDefault(t => t.success)
					.success);

			return propertyDeclarations;
		}
		private Control GetControl(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var semanticModel = GetSemanticModel(typeDeclaration);
			var modelTypes = typeDeclaration.AttributeLists
					.OfAttributeClasses(semanticModel, Attributes.FallbackControl)
					.Select(a => (success: Attributes.Factories.FallbackControl.TryBuild(a, semanticModel, out var attribute), attribute))
					.Where(t => t.success)
					.Select(t => t.attribute.GetTypeParameter("modelType") as ITypeIdentifier);

			var identifier = GetType(typeDeclaration);

			var template = Control.Create(identifier)
				.WithRange(modelTypes);

			return template;
		}
		private Template GetTemplate(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var semanticModel = GetSemanticModel(typeDeclaration);
			var modelTypes = typeDeclaration.AttributeLists
					.OfAttributeClasses(semanticModel, Attributes.FallbackTemplate)
					.Select(a => (success: Attributes.Factories.FallbackTemplate.TryBuild(a, semanticModel, out var attribute), attribute))
					.Where(t => t.success)
					.Select(t => t.attribute.GetTypeParameter("modelType") as ITypeIdentifier);

			var identifier = GetType(typeDeclaration);

			var template = Template.Create(identifier).WithRange(modelTypes);

			return template;
		}
		#endregion
		private TypeIdentifier GetType(TypeSyntax type)
		{
			var semanticModel = _compilation.GetSemanticModel(type.SyntaxTree);
			var symbol = semanticModel.GetDeclaredSymbol(type) as ITypeSymbol ??
						 semanticModel.GetTypeInfo(type).Type;

			var identifier = TypeIdentifier.Create(symbol);

			return identifier;
		}
		private TypeIdentifier GetType(BaseTypeDeclarationSyntax declaration)
		{
			var semanticModel = _compilation.GetSemanticModel(declaration.SyntaxTree);
			var symbol = semanticModel.GetDeclaredSymbol(declaration) as ITypeSymbol ??
						 semanticModel.GetTypeInfo(declaration).Type;

			var identifier = TypeIdentifier.Create(symbol);

			return identifier;
		}
		private SemanticModel GetSemanticModel(SyntaxNode node)
		{
			var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree);

			return semanticModel;
		}
	}
}
