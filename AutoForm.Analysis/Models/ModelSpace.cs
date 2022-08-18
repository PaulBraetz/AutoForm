using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoForm.Analysis.Models
{
	public readonly struct ModelSpace : IEquatable<ModelSpace>
	{
		public readonly IEnumerable<Model> Models;
		public readonly IEnumerable<FallbackControl> FallbackControls;
		public readonly IEnumerable<FallbackTemplate> FallbackTemplates;
		private readonly String _json;
		private readonly String _string;

		private ModelSpace(IEnumerable<Model> models, IEnumerable<FallbackControl> controls, IEnumerable<FallbackTemplate> templates)
		{
			models.ThrowOnDuplicate(TypeIdentifierName.ModelAttribute.ToString());
			controls.ThrowOnDuplicate(TypeIdentifierName.FallbackControlAttribute.ToString());
			templates.ThrowOnDuplicate(TypeIdentifierName.FallbackTemplateAttribute.ToString());

			Models = models.ToArray();
			FallbackControls = controls.ToArray();
			FallbackTemplates = templates.ToArray();

			_json = Json.Object(Json.KeyValuePair(nameof(Models), Models),
								Json.KeyValuePair(nameof(FallbackControls), FallbackControls),
								Json.KeyValuePair(nameof(FallbackTemplates), FallbackTemplates));
			_string = _json;
		}

		public static ModelSpace Create()
		{
			return new ModelSpace(Array.Empty<Model>(), Array.Empty<FallbackControl>(), Array.Empty<FallbackTemplate>());
		}

		public ModelSpace AppendRange(IEnumerable<Model> models)
		{
			return new ModelSpace(Models.Concat(models), FallbackControls, FallbackTemplates);
		}

		public ModelSpace AppendRange(IEnumerable<FallbackControl> controls)
		{
			return new ModelSpace(Models, FallbackControls.Concat(controls), FallbackTemplates);
		}

		public ModelSpace AppendRange(IEnumerable<FallbackTemplate> templates)
		{
			return new ModelSpace(Models, FallbackControls, FallbackTemplates.Concat(templates));
		}

		private IEnumerable<Property> ApplyFallbacksToProperties(IEnumerable<Property> properties, IEnumerable<Model> models, IEnumerable<FallbackControl> fallbackControls)
		{
			foreach (Property property in properties)
			{
				TypeIdentifier control = property.Control;
				if (control == default)
				{
					control = models.SingleOrDefault(m => m.Name == property.Type).Control;
					if (control == default)
					{
						control = fallbackControls.SingleOrDefault(c => c.Models.Any(m => m == property.Type)).Name;
					}
				}

				TypeIdentifier template = property.Template;
				if (template == default)
				{
					template = models.SingleOrDefault(m => m.Name == property.Type).Template;
					if (template == default)
					{
						template = FallbackTemplates.SingleOrDefault(t => t.Models.Any(m => m == property.Type)).Name;
					}
				}

				yield return Property.Create(property.Name, property.Type, control, template, property.Order);
			}
		}

		private IEnumerable<Model> ApplyFallbacksToModels(IEnumerable<Model> models, IEnumerable<FallbackControl> fallbackControls)
		{
			var fallbackAppliedOnceModels = new List<Model>();

			foreach (Model model in models)
			{
				TypeIdentifier control = model.Control;
				if (control == default)
				{
					control = fallbackControls.SingleOrDefault(c => c.Models.Any(m => m == model.Name)).Name;
				}

				TypeIdentifier template = model.Template;
				if (template == default)
				{
					template = FallbackTemplates.SingleOrDefault(t => t.Models.Any(m => m == model.Name)).Name;
				}

				Model fallbackAppliedModel = Model.Create(model.Name, control, template, model.AttributesProvider).AppendRange(model.Properties);

				fallbackAppliedOnceModels.Add(fallbackAppliedModel);
			}

			var fallbackAppliedOnceModelControls = fallbackAppliedOnceModels.Select(FallbackControl.Create);
			fallbackControls = fallbackControls.Where(c1 => !fallbackAppliedOnceModelControls.Any(c2=>c2.Name == c1.Name)).Concat(fallbackAppliedOnceModelControls);

			foreach (var model in fallbackAppliedOnceModels)
			{
				IEnumerable<Property> properties = ApplyFallbacksToProperties(model.Properties, fallbackAppliedOnceModels, fallbackControls);

				Model fallbackAppliedTwiceModel = Model.Create(model.Name,
												   model.Control,
												   model.Template,
												   model.AttributesProvider).AppendRange(properties);

				yield return fallbackAppliedTwiceModel;
			}
		}

		public ModelSpace ApplyFallbacks()
		{
			IEnumerable<FallbackControl> fallbackControls = FallbackControls.Concat(Models.Select(FallbackControl.Create).Where(c1=>!FallbackControls.Any(c2=>c2.Name == c1.Name)));
			IEnumerable<Model> fallbackAppliedModels = ApplyFallbacksToModels(Models, fallbackControls);

			ModelSpace modelSpace = Create()
				.AppendRange(fallbackAppliedModels)
				.AppendRange(fallbackControls)
				.AppendRange(FallbackTemplates);

			return modelSpace;
		}

		public override String ToString()
		{
			return _json ?? "null";
		}
		public String ToEscapedString()
		{
			return _string ?? String.Empty;
		}

		public override Boolean Equals(Object obj)
		{
			return obj is ModelSpace space && Equals(space);
		}

		public Boolean Equals(ModelSpace other)
		{
			return _json == other._json;
		}

		public override Int32 GetHashCode()
		{
			return -992964542 + EqualityComparer<String>.Default.GetHashCode(_json);
		}

		public static Boolean operator ==(ModelSpace left, ModelSpace right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(ModelSpace left, ModelSpace right)
		{
			return !(left == right);
		}
	}
}
