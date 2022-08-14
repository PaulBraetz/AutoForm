using AutoForm.Generate.Source;
using AutoForm.Generate.Source.Models;
using System;

namespace AutoForm.Generate.Blazor
{
	public sealed partial class BlazorSourceGenerator : IControlsSourceGenerator
	{
		public String Generate(ModelSpace modelSpace)
		{
			throw new NotImplementedException();
		}

		public String Generate(Error error)
		{
			throw new NotImplementedException();
		}

		#region Placeholders
		private const String GENERATED_DATE = "{" + nameof(GENERATED_DATE) + "}";
		private const String MODEL_CONTROL_PAIRS = "{" + nameof(MODEL_CONTROL_PAIRS) + "}";
		private const String DEFAULT_CONTROLS = "{" + nameof(DEFAULT_CONTROLS) + "}";
		private const String CONTROLS = "{" + nameof(CONTROLS) + "}";

		private const String CONTROL_TYPE = "{" + nameof(CONTROL_TYPE) + "}";
		private const String CONTROL_TYPE_IDENTIFIER = "{" + nameof(CONTROL_TYPE_IDENTIFIER) + "}";

		private const String SUB_CONTROL_PROPERTY_IDENTIFIER = "{" + nameof(SUB_CONTROL_PROPERTY_IDENTIFIER) + "}";
		private const String SUB_CONTROL_PROPERTY = "{" + nameof(SUB_CONTROL_PROPERTY) + "}";
		private const String SUB_CONTROL_PROPERTIES = "{" + nameof(SUB_CONTROL_PROPERTIES) + "}";
		private const String SUB_CONTROL_LINE_INDEX = "{" + nameof(SUB_CONTROL_LINE_INDEX) + "}";
		private const String SUB_CONTROL_PASS_ATTRIBUTES = "{" + nameof(SUB_CONTROL_PASS_ATTRIBUTES) + "}";
		private const String SUB_CONTROL_TYPE = "{" + nameof(SUB_CONTROL_TYPE) + "}";

		private const String MODEL_TYPE = "{" + nameof(MODEL_TYPE) + "}";

		private const String SUB_CONTROLS = "{" + nameof(SUB_CONTROLS) + "}";

		private const String PROPERTY_TYPE = "{" + nameof(PROPERTY_TYPE) + "}";
		private const String PROPERTY_IDENTIFIER = "{" + nameof(PROPERTY_IDENTIFIER) + "}";

		private const String ATTRIBUTES_PROVIDER_IDENTIFIER = "{" + nameof(ATTRIBUTES_PROVIDER_IDENTIFIER) + "}";

		private const String ERROR_MESSAGE = "{" + nameof(ERROR_MESSAGE) + "}";
		#endregion

		#region Methods
		private static String GetError(ErrorModel error)
		{
			return GetErrorTemplate(error).Build();
		}

		private static ErrorTemplate GetErrorTemplate(ErrorModel error)
		{
			return new ErrorTemplate()
				.WithMessage(error.Message);
		}

		private static String GetControls(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			return GetControlsTemplate(models, controls).Build();
		}

		private static ControlsTemplate GetControlsTemplate(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			IEnumerable<ModelControlPairTemplate> modelControlPairTemplates = GetModelControlPairTemplates(models, controls);
			IEnumerable<ControlTemplate> controlTemplates = GetControlTemplates(models, controls);
			DefaultControlsTemplate defaultControlsTemplate = GetDefaultControlsTemplate(controls);

			return new ControlsTemplate()
				.WithControlTemplates(controlTemplates)
				.WithDefaultControlsTemplate(defaultControlsTemplate)
				.WithModelControlPairTemplates(modelControlPairTemplates);
		}

		private static DefaultControlsTemplate GetDefaultControlsTemplate(IEnumerable<ControlModel> controls)
		{
			IEnumerable<String> requiredDefaulControlTypes = GetRequiredDefaultControlTypes(controls);

			return new DefaultControlsTemplate()
				.WithRequiredDefaultControls(requiredDefaulControlTypes);
		}
		private static IEnumerable<ModelControlPairTemplate> GetModelControlPairTemplates(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			IEnumerable<String> requiredDefaulControlTypes = GetRequiredDefaultControlTypes(controls);

			return requiredDefaulControlTypes.Select(GetModelControlPairTemplate)
				.Concat(GetConcatenatedControlModels(models, controls)
				.SelectMany(GetModelControlPairTemplates));
		}
		private static ModelControlPairTemplate GetModelControlPairTemplate(String requiredDefaultControlType)
		{
			return new ModelControlPairTemplate()
				.WithControlType(DefaultControlsTemplate.AvailableDefaultControls[requiredDefaultControlType])
				.WithModelType(requiredDefaultControlType);
		}
		private static IEnumerable<String> GetRequiredDefaultControlTypes(IEnumerable<ControlModel> controls)
		{
			return DefaultControlsTemplate.AvailableDefaultControls.Keys.Except(controls.SelectMany(c => c.ModelTypes));
		}
		private static IEnumerable<ModelControlPairTemplate> GetModelControlPairTemplates(ControlModel control)
		{
			return control.ModelTypes
					.Select(t => new ModelControlPairTemplate()
						.WithModelType(t)
						.WithControlType(control.Type));
		}

		private static IEnumerable<ControlTemplate> GetControlTemplates(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			IEnumerable<ControlModel> allControlModels = GetConcatenatedControlModels(models, controls);
			IEnumerable<ModelModel> modelsWithMissingControl = GetModelModelsWithMissingControlModel(models, controls);

			var result = new List<ControlTemplate>();
			var exceptions = new List<Exception>();
			foreach (var model in modelsWithMissingControl)
			{
				try
				{
					result.Add(GetControlTemplate(model, allControlModels));
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}
			}

			if (exceptions.Any())
			{
				var message = String.Join("\n\n", exceptions.Select(e => e.Message));
				throw new AggregateException(message, exceptions);
			}

			return result;
		}
		private static ControlTemplate GetControlTemplate(ModelModel model, IEnumerable<ControlModel> controls)
		{
			var controlTypeIdentifierTemplate = GetControlTypeIdentifierTemplate(model);
			var subControlTemplates = GetSubControlTemplates(model, controls);
			var subControlPropertyTemplates = GetSubControlPropertyTemplates(model);

			return new ControlTemplate()
				.WithModelType(model.Type)
				.WithControlTypeIdentifierTemplate(controlTypeIdentifierTemplate)
				.WithSubControlTemplates(subControlTemplates)
				.WithSubControlPropertyTemplates(subControlPropertyTemplates);
		}
		private static IEnumerable<SubControlPropertyTemplate> GetSubControlPropertyTemplates(ModelModel model)
		{
			return model.Properties.Select(GetSubControlPropertyTemplate);
		}
		private static SubControlPropertyTemplate GetSubControlPropertyTemplate(PropertyModel property)
		{
			SubControlPropertyIdentifierTemplate subControlPropertyIdentifierTemplate = GetSubControlPropertyIdentifierTemplate(property);

			return new SubControlPropertyTemplate()
				.WithSubControlPropertyIdentifierTemplate(subControlPropertyIdentifierTemplate)
				.WithPropertyIdentifier(property.Identifier)
				.WithPropertyType(property.Type);
		}
		private static ControlTypeIdentifierTemplate GetControlTypeIdentifierTemplate(ModelModel model)
		{
			return new ControlTypeIdentifierTemplate()
				.WithModelType(model.Type);
		}
		private static IEnumerable<SubControlTemplate> GetSubControlTemplates(ModelModel model, IEnumerable<ControlModel> controls)
		{
			var exceptions = new List<Exception>();
			var templates = new List<SubControlTemplate>();
			foreach (var property in model.Properties)
			{
				try
				{
					templates.Add(GetSubControlTemplate(model, property, controls));
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}
			}

			if (exceptions.Any())
			{
				var message = $"Error while generating control for {model.Type}:\n{String.Join("\n", exceptions.Select(e => e.Message))}";
				throw new AggregateException(message, exceptions);
			}

			return templates;
		}
		private static SubControlTemplate GetSubControlTemplate(ModelModel model, PropertyModel property, IEnumerable<ControlModel> controls)
		{
			var controlType = String.IsNullOrWhiteSpace(property.ControlType) ? controls.SingleOrDefault(c => c.ModelTypes.Contains(property.Type)).Type : property.ControlType;

			if (controlType == null && !DefaultControlsTemplate.AvailableDefaultControls.TryGetValue(property.Type, out controlType))
			{
				throw new Exception($"Unable to locate control for {property.Identifier}. Make sure a control for {property.Type} is registered.");
			}

			var subControlPassAttributesTemplate = GetSubControlPassAttributesTemplate(model);
			var subControlPropertyIdentifierTemplate = GetSubControlPropertyIdentifierTemplate(property);

			return new SubControlTemplate()
				.WithModelType(model.Type)
				.WithPropertyIdentifier(property.Identifier)
				.WithPropertyType(property.Type)
				.WithSubControlType(controlType)
				.WithSubControlPassAttributesTemplate(subControlPassAttributesTemplate)
				.WithSubControlPropertyIdentifierTemplate(subControlPropertyIdentifierTemplate);
		}

		private static SubControlPassAttributesTemplate GetSubControlPassAttributesTemplate(ModelModel model)
		{
			return new SubControlPassAttributesTemplate()
				.WithAttributesProviderIdentifier(model.AttributesProviderIdentifier);
		}

		private static SubControlPropertyIdentifierTemplate GetSubControlPropertyIdentifierTemplate(PropertyModel property)
		{
			return new SubControlPropertyIdentifierTemplate()
				.WithPropertyIdentifier(property.Identifier);
		}

		private static IEnumerable<ControlModel> GetConcatenatedControlModels(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			foreach (var control in controls)
			{
				yield return control;
			}

			IEnumerable<ModelModel> modelModelsWithMissingControlModels = GetModelModelsWithMissingControlModel(models, controls);

			foreach (var model in modelModelsWithMissingControlModels)
			{
				yield return GetControlModel(model);
			}
		}
		private static IEnumerable<ModelModel> GetModelModelsWithMissingControlModel(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			return models.Where(m => !controls.Any(c => c.ModelTypes.Contains(m.Type)));
		}
		private static ControlModel GetControlModel(ModelModel model)
		{
			ControlTypeIdentifierTemplate controlIdentifierTemplate = GetControlTypeIdentifierTemplate(model);
			return new ControlModel(controlIdentifierTemplate.Build(), new[] { model.Type });
		}

		#endregion
	}
}
