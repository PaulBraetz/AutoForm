using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoForm.Analysis
{
	internal sealed class ModelExtractor
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
			var models = _data.DefaultModels.Select(GetModel);

			return models;
		}
		private IEnumerable<Control> GetControls()
		{
			var controlDeclarations = _data.Controls;
			var controls = controlDeclarations.Select(GetControl);

			return controls;
		}
		private IEnumerable<Template> GetTemplates()
		{
			var templateDeclarations = _data.DefaultTemplates;
			var templates = templateDeclarations.Select(GetTemplate);

			return templates;
		}

		private Model GetModel(BaseTypeDeclarationSyntax typeDeclaration)
		{
			var properties = GetProperties(typeDeclaration);
			var identifier = GetTypeIdentifier(typeDeclaration);
			var baseModels = GetBaseModels(typeDeclaration);
			var baseProperties = GetBaseProperties(typeDeclaration);

			var model = Model.Create(identifier, baseModels, baseProperties)
				.AddProperties(properties);

			return model;
		}
		private PropertyIdentifier[] GetBaseProperties(BaseTypeDeclarationSyntax modelDeclaration)
		{
			var semanticModel = GetSemanticModel(modelDeclaration);
			var baseProperties = modelDeclaration.AttributeLists
				.SelectMany(l => l.Attributes)
				.Select(a => (success: Attributes.Factories.SubModel.TryBuild(a, semanticModel, out var attribute), attribute))
				.Where(t => t.success)
				.Select(t => (type: t.attribute.GetTypeParameter("baseModel") as ITypeIdentifier, members: t.attribute.Members))
				.SelectMany(t => t.members.Select(m => PropertyIdentifier.Create(m, t.type)))
				.ToArray();

			return baseProperties;
		}
		private ITypeIdentifier[]  GetBaseModels(BaseTypeDeclarationSyntax modelDeclaration)
		{
			var semanticModel = GetSemanticModel(modelDeclaration);
			var baseModels = modelDeclaration.AttributeLists
				.SelectMany(l => l.Attributes)
				.Select(a => (success: Attributes.Factories.SubModel.TryBuild(a, semanticModel, out var attribute), attribute))
				.Where(t => t.success && !t.attribute.Members.Any())
				.Select(t => t.attribute.GetTypeParameter("baseModel") as ITypeIdentifier)
				.Where(i => i != null)
				.ToArray();

			return baseModels;
		}
		private Property GetProperty(PropertyDeclarationSyntax propertyDeclaration, BaseTypeDeclarationSyntax modelDeclaration)
		{
			var semanticModel = GetSemanticModel(propertyDeclaration);
			var modelPropertyAttribute = propertyDeclaration.AttributeLists
				.OfAttributeClasses(semanticModel, Attributes.ModelProperty)
				.Select(a => (success: Attributes.Factories.ModelProperty.TryBuild(a, semanticModel, out var attribute), attribute))
				.Single(t => t.success)
				.attribute;

			var modelIdentifier = GetTypeIdentifier(modelDeclaration);

			var identifier = PropertyIdentifier.Create(propertyDeclaration.Identifier.ToString(), modelIdentifier);
			var type = GetTypeIdentifier(propertyDeclaration.Type);

			var property = Property.Create(identifier, type);

			return property;
		}
		private IEnumerable<Property> GetProperties(BaseTypeDeclarationSyntax modelDeclaration)
		{
			var propertyDeclarations = GetPropertyDeclarations(modelDeclaration);
			var properties = propertyDeclarations.Select(p => GetProperty(p, modelDeclaration));

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
		private Control GetControl(BaseTypeDeclarationSyntax controlDeclaration)
		{
			var semanticModel = GetSemanticModel(controlDeclaration);
			var targets = controlDeclaration.AttributeLists
					.OfAttributeClasses(semanticModel, Attributes.DefaultControl)
					.Select(a => (success: Attributes.Factories.DefaultControl.TryBuild(a, semanticModel, out var attribute), attribute))
					.Where(t => t.success)
					.Select(t => (model: t.attribute.GetTypeParameter("modelType") as ITypeIdentifier, members: t.attribute.Members.ToArray()));

			var identifier = GetTypeIdentifier(controlDeclaration);

			var models = targets
				.Where(t => t.members.Length == 0)
				.Select(t => t.model);
			var properties = targets
				.Where(t => t.members.Length > 0)
				.SelectMany(t => t.members.Select(m => PropertyIdentifier.Create(m, t.model)));

			var control = Control.Create(identifier).WithModels(models).WithProperties(properties);

			return control;
		}
		private Template GetTemplate(BaseTypeDeclarationSyntax templateDeclaration)
		{
			var semanticModel = GetSemanticModel(templateDeclaration);
			var targets = templateDeclaration.AttributeLists
					.OfAttributeClasses(semanticModel, Attributes.DefaultTemplate)
					.Select(a => (success: Attributes.Factories.DefaultTemplate.TryBuild(a, semanticModel, out var attribute), attribute))
					.Where(t => t.success)
					.Select(t => (model: t.attribute.GetTypeParameter("modelType") as ITypeIdentifier, members: t.attribute.Members.ToArray()));

			var identifier = GetTypeIdentifier(templateDeclaration);

			var models = targets
				.Where(t => t.members.Length == 0)
				.Select(t => t.model);
			var properties = targets
				.Where(t => t.members.Length > 0)
				.SelectMany(t => t.members.Select(m => PropertyIdentifier.Create(m, t.model)));

			var template = Template.Create(identifier).WithModels(models).WithProperties(properties);

			return template;
		}
		#endregion
		private ITypeIdentifier GetTypeIdentifier(TypeSyntax type)
		{
			var semanticModel = _compilation.GetSemanticModel(type.SyntaxTree);
			var symbol = semanticModel.GetDeclaredSymbol(type) as ITypeSymbol ??
						 semanticModel.GetTypeInfo(type).Type;

			var identifier = TypeIdentifier.Create(symbol);

			return identifier;
		}
		private ITypeIdentifier GetTypeIdentifier(BaseTypeDeclarationSyntax declaration)
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
