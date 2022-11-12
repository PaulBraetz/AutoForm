using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

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
			var propertyResolvedModels = Models
				.Select(m =>
					Model.Create(m.Name, m.BaseModels)
					.WithControl(m.Control)
					.WithTemplate(m.Template)
					.WithProperties(ResolveProperties(m, modelDict)))
				.ToArray();

			var defaultAppliedModels = applyDefaults(propertyResolvedModels, Templates);

			return defaultAppliedModels;

			IEnumerable<Model> applyDefaults(IEnumerable<Model> models, IEnumerable<Template> templates)
			{
				foreach (var model in models)
				{
					var control = availableControls.SingleOrDefault(c => c.Models.Contains(model.Name)).Name;
					var template = templates.SingleOrDefault(t => t.Models.Contains(model.Name)).Name;

					var defaultAppliedProperties = new List<Property>();

					foreach (var property in model.Properties)
					{
						var subControl = availableControls.SingleOrDefault(c => c.Properties.Contains(property.Name)).Name ??
							availableControls.SingleOrDefault(c => c.Models.Contains(property.Type)).Name;

						var subTemplate = templates.SingleOrDefault(c => c.Properties.Contains(property.Name)).Name ??
							templates.SingleOrDefault(c => c.Models.Contains(property.Type)).Name;

						var defaultAppliedProperty = property.WithControl(subControl).WithTemplate(subTemplate);

						defaultAppliedProperties.Add(defaultAppliedProperty);
					}

					var defaultAppliedModel = Model.Create(model.Name, model.BaseModels)
						.WithControl(control)
						.WithTemplate(template)
						.WithProperties(defaultAppliedProperties);

					yield return defaultAppliedModel;
				}
			}
		}

		private static ISet<Property> ResolveProperties(Model model, IDictionary<ITypeIdentifier, Model> modelDict)
		{
			var baseModels = new HashSet<ITypeIdentifier>();
			var properties = new HashSet<Property>(PropertyEqualityComparer.Instance);

			resolveProperties(model);

			return properties;

			void resolveProperties(Model baseModel)
			{
				if (baseModels.Add(baseModel.Name))
				{
					foreach (var baseBaseModelIdentifier in baseModel.BaseModels)
					{
						if (modelDict.TryGetValue(baseBaseModelIdentifier, out var baseBaseModel))
						{
							resolveProperties(baseBaseModel);
						}
						else
						{
							throw new Exception($"While attempting to resolve properties for {model.Name}: base model {baseBaseModelIdentifier} is not a model.");
						}
					}
					foreach (var property in baseModel.Properties)
					{
						properties.Add(property);
					}
				}
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
