using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoForm.Analysis
{
	internal readonly struct ModelSpace
	{
		private sealed class PropertyEqualityComparer : IEqualityComparer<Property>
		{
			public static readonly PropertyEqualityComparer Instance = new PropertyEqualityComparer();

			public Boolean Equals(Property x, Property y)
			{
				return x.Name.Equals(y.Name);
			}

			public Int32 GetHashCode(Property obj)
			{
				return obj.Name.GetHashCode();
			}
		}

		public readonly Model[] Models;
		public readonly Control[] Controls;
		public readonly Template[] Templates;
		public readonly Control[] RequiredGeneratedControls;

		private ModelSpace(Model[] models, Control[] controls, Template[] templates, Control[] requiredGeneratedControls)
		{
			models.ThrowOnDuplicate(nameof(models));
			controls.ThrowOnDuplicate(Attributes.DefaultControl);
			templates.ThrowOnDuplicate(Attributes.DefaultTemplate);
			requiredGeneratedControls.ThrowOnDuplicate("required generated control");

			Models = models ?? Array.Empty<Model>();
			Controls = controls ?? Array.Empty<Control>();
			Templates = templates ?? Array.Empty<Template>();
			RequiredGeneratedControls = requiredGeneratedControls ?? Array.Empty<Control>();
		}

		public static ModelSpace Create()
		{
			return new ModelSpace(Array.Empty<Model>(), Array.Empty<Control>(), Array.Empty<Template>(), Array.Empty<Control>());
		}

		public ModelSpace WithModels(IEnumerable<Model> models)
		{
			return new ModelSpace(Models.Concat(models).ToArray(), Controls, Templates, RequiredGeneratedControls);
		}

		public ModelSpace WithFallbackControls(IEnumerable<Control> controls)
		{
			return new ModelSpace(Models, Controls.Concat(controls).ToArray(), Templates, RequiredGeneratedControls);
		}

		public ModelSpace WithTemplates(IEnumerable<Template> templates)
		{
			return new ModelSpace(Models, Controls, Templates.Concat(templates).ToArray(), RequiredGeneratedControls);
		}

		private ModelSpace WithRequiredGeneratedControls(IEnumerable<Control> requiredGeneratedControls)
		{
			return new ModelSpace(Models, Controls, Templates, RequiredGeneratedControls.Concat(requiredGeneratedControls).ToArray());
		}

		private IEnumerable<Model> ApplyDefaultsToModels()
		{
			var generatedDefaultControls = Models.Select(m => Control.CreateGenerated(m.Name));
			var availableControls = Controls.Where(c1 => !generatedDefaultControls.Any(c2 => c2.Name == c1.Name)).Concat(generatedDefaultControls);

			var modelDict = Models.ToDictionary(m => m.Name, m => m);

			var propertyControls = availableControls
				.SelectMany(c => c.Properties.ToDictionary(p => p, p => c.Name))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			var propertyTemplates = Templates
				.SelectMany(t => t.Properties.ToDictionary(p => p, p => t.Name))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			var modelControls = availableControls
				.SelectMany(c => c.Models.ToDictionary(m => m, p => c.Name))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			var modelTemplates = Templates
				.SelectMany(t => t.Models.ToDictionary(m => m, p => t.Name))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			var basePropertyMap = Models
				.SelectMany(m => m.Properties.ToDictionary(p => p.Name, p => p))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			var defaultAppliedModels = Models
				.Select(resolveBaseProperties)
				.Select(resolveProperties)
				.Select(m => modelControls.TryGetValue(m.Name, out var control) ? m.WithControl(control) : m)
				.Select(m => modelTemplates.TryGetValue(m.Name, out var template) ? m.WithTemplate(template) : m);

			return defaultAppliedModels;

			Model resolveBaseProperties(Model model)
			{
				var baseModels = new HashSet<ITypeIdentifier>();
				var baseProperties = new HashSet<PropertyIdentifier>();

				resolve(model);

				model = model.WithBaseProperties(baseProperties);

				return model;

				void resolve(Model baseModel)
				{
					if (baseModels.Add(baseModel.Name))
					{
						foreach (var baseBaseModelIdentifier in baseModel.BaseModels)
						{
							if (modelDict.TryGetValue(baseBaseModelIdentifier, out var baseBaseModel))
							{
								resolve(baseBaseModel);
							}
							else
							{
								throw new Exception($"While attempting to resolve properties for {model.Name}: base model {baseBaseModelIdentifier} is not a model.");
							}
						}

						foreach (var property in baseModel.Properties)
						{
							baseProperties.Add(property.Name);
						}

						foreach (var baseProperty in baseModel.BaseProperties)
						{
							baseProperties.Add(baseProperty);
						}
					}
				}
			}

			Model resolveProperties(Model model)
			{
				var properties = new HashSet<Property>(PropertyEqualityComparer.Instance);

				foreach (var propertyName in model.BaseProperties)
				{
					if (!basePropertyMap.TryGetValue(propertyName, out var property))
					{
						throw new Exception($"Unable to resolve base property {propertyName.Model}.{propertyName.Name} for model {model.Name}.");
					}

					var inheritedPropertyName = property.Name.WithModel(model.Name);
					var propertyType = property.Type;

					if (propertyControls.TryGetValue(inheritedPropertyName, out var control) ||
						modelControls.TryGetValue(propertyType, out control))
					{
						property = property.WithControl(control);
					}

					if (propertyTemplates.TryGetValue(inheritedPropertyName, out var template) ||
						modelTemplates.TryGetValue(propertyType, out template))
					{
						property = property.WithTemplate(template);
					}

					properties.Add(property);
				}

				model = model.RedefineProperties(properties);

				return model;
			}
		}

		public ModelSpace WithRequiredGeneratedControls(bool checkModelViability = true)
		{
			var defaultAppliedModels = ApplyDefaultsToModels();

			var requiredGeneratedControls = defaultAppliedModels
				.Where(m => m.Control.IsGenerated())
				.Select(m => m.Name)
				.Concat(defaultAppliedModels
					.SelectMany(m => m.Properties)
					.Where(p => p.Control == default || p.Control.IsGenerated())
					.Select(p => p.Type))
				.Distinct()
				.Select(Control.CreateGenerated);

			if (checkModelViability)
			{
				var exceptions = new List<Exception>();
				foreach (var requiredControlModel in requiredGeneratedControls.SelectMany(c => c.Models))
				{
					if (!defaultAppliedModels.Any(m => m.Name == requiredControlModel))
					{
						exceptions.Add(new ArgumentException($"Unable to provide control for {requiredControlModel}"));
					}
				}

				if (exceptions.Any())
				{
					throw new AggregateException(exceptions);
				}
			}

			var modelSpace = Create()
				.WithModels(defaultAppliedModels)
				.WithFallbackControls(Controls)
				.WithTemplates(Templates)
				.WithRequiredGeneratedControls(requiredGeneratedControls);

			return modelSpace;
		}
	}
}
