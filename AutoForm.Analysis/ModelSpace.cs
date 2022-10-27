using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
	internal readonly struct ModelSpace
	{
		public readonly Model[] Models;
		public readonly Control[] Controls;
		public readonly Template[] Templates;
		public readonly Control[] RequiredGeneratedControls;

		private ModelSpace(Model[] models, Control[] controls, Template[] templates, Control[] requiredGeneratedControls)
		{
			models.ThrowOnDuplicate(nameof(models));
			controls.ThrowOnDuplicate(Attributes.Control);
			templates.ThrowOnDuplicate(Attributes.FallbackTemplate);
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

		private IEnumerable<Model> ApplyFallbacksToModels()
		{
			var generatedFallbackControls = Models.Select(m => Control.CreateGenerated(m.Name));
			var availableControls = Controls.Where(c1 => !generatedFallbackControls.Any(c2 => c2.Name == c1.Name)).Concat(generatedFallbackControls);

			var fallbackAppliedModels = new List<Model>();

			foreach (var model in Models)
			{
				var control = model.Control;
				if (control == default)
				{
					control = availableControls.SingleOrDefault(c => c.Models.Contains(model.Name)).Name;
				}

				var template = model.Template;
				if (template == default)
				{
					template = Templates.SingleOrDefault(t => t.Models.Contains(model.Name)).Name;
				}

				var fallbackAppliedProperties = new List<Property>();

				foreach (var property in model.Properties)
				{
					var subControl = property.Control;
					if (subControl == default)
					{
						subControl = availableControls.SingleOrDefault(c => c.Models.Contains(property.Type)).Name;
					}

					var subTemplate = property.Template;
					if (subTemplate == default)
					{
						subTemplate = Templates.SingleOrDefault(t => t.Models.Contains(property.Type)).Name;
					}

					var fallbackAppliedProperty = Property.Create(property.Name, property.Type, subControl, subTemplate, property.Order);

					fallbackAppliedProperties.Add(fallbackAppliedProperty);
				}

				var fallbackAppliedModel = Model.Create(model.Name, control, template, model.AttributesProvider).WithRange(fallbackAppliedProperties);

				fallbackAppliedModels.Add(fallbackAppliedModel);
			}

			return fallbackAppliedModels;
		}

		public ModelSpace WithRequiredGeneratedControls(bool checkModelViability = false)
		{
			var fallbackAppliedModels = ApplyFallbacksToModels();

			var requiredGeneratedControls = fallbackAppliedModels
				.Where(m => m.Control.IsGenerated())
				.Select(m => m.Name)
				.Concat(fallbackAppliedModels
					.SelectMany(m => m.Properties)
					.Where(p => p.Control.IsGenerated())
					.Select(p => p.Type))
				.Distinct()
				.Select(Control.CreateGenerated);

			if (checkModelViability)
			{
				var exceptions = new List<Exception>();
				foreach (var requiredControlModel in requiredGeneratedControls.SelectMany(c => c.Models))
				{
					if (!fallbackAppliedModels.Any(m => m.Name == requiredControlModel))
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
				.WithModels(fallbackAppliedModels)
				.WithFallbackControls(Controls)
				.WithTemplates(Templates)
				.WithRequiredGeneratedControls(requiredGeneratedControls);

			return modelSpace;
		}
	}
}
