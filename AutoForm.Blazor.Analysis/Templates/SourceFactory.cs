using AutoForm.Analysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate.Blazor.Templates
{
	internal sealed partial class SourceFactory
	{
		#region Placeholders
		private const String GENERATED_DATE = "{" + nameof(GENERATED_DATE) + "}";
		private const String MODEL_CONTROL_PAIRS = "{" + nameof(MODEL_CONTROL_PAIRS) + "}";
		private const String MODEL_TEMPLATE_PAIRS = "{" + nameof(MODEL_TEMPLATE_PAIRS) + "}";
		private const String DEFAULT_CONTROLS = "{" + nameof(DEFAULT_CONTROLS) + "}";
		private const String CONTROLS = "{" + nameof(CONTROLS) + "}";

		private const String CONTROL_TYPE = "{" + nameof(CONTROL_TYPE) + "}";
		private const String CONTROL_TYPE_IDENTIFIER = "{" + nameof(CONTROL_TYPE_IDENTIFIER) + "}";

		private const String TEMPLATE_TYPE = "{" + nameof(TEMPLATE_TYPE) + "}";

		private const String LINE_INDEX = "{" + nameof(LINE_INDEX) + "}";
		private const String SUB_CONTROL_PASS_ATTRIBUTES = "{" + nameof(SUB_CONTROL_PASS_ATTRIBUTES) + "}";

		private const String MODEL_TYPE = "{" + nameof(MODEL_TYPE) + "}";

		private const String SUB_CONTROLS = "{" + nameof(SUB_CONTROLS) + "}";

		private const String PROPERTY_TYPE = "{" + nameof(PROPERTY_TYPE) + "}";
		private const String PROPERTY_IDENTIFIER = "{" + nameof(PROPERTY_IDENTIFIER) + "}";

		private const String ATTRIBUTES_PROVIDER_IDENTIFIER = "{" + nameof(ATTRIBUTES_PROVIDER_IDENTIFIER) + "}";

		private const String ERROR_MESSAGE = "{" + nameof(ERROR_MESSAGE) + "}";
		#endregion

		private readonly ModelSpace _modelSpace;
		private readonly Error _error;
		private readonly Boolean _isError;

		private SourceFactory(ModelSpace modelSpace, Error error, Boolean isError)
		{
			if (!isError)
			{
				IEnumerable<FallbackControl> defaultFallbackControls = DefaultControlsTemplate.DefaultModelControlPairs
					.Where(kvp => !modelSpace.FallbackControls.SelectMany(c => c.Models).Contains(kvp.Key))
					.GroupBy(kvp => kvp.Value)
					.Select(group => FallbackControl.Create(group.Key).AppendRange(group.Select(kvp => kvp.Key)));

				modelSpace = modelSpace.AppendRange(defaultFallbackControls).ApplyFallbacks();
			}

			_modelSpace = modelSpace;
			_error = error;
			_isError = isError;
		}

		public static SourceFactory Create(ModelSpace modelSpace)
		{
			return new SourceFactory(modelSpace, default, false);
		}
		public static SourceFactory Create(Error error)
		{
			return new SourceFactory(default, error, true);
		}

		public String Build()
		{
			return _isError ?
				GetError() :
				GetControls();
		}

		#region Methods
		private String GetError()
		{
			return GetErrorTemplate().Build();
		}

		private ErrorTemplate GetErrorTemplate()
		{
			return new ErrorTemplate()
				.WithError(_error);
		}

		private String GetControls()
		{
			return GetControlsTemplate().Build();
		}

		private ControlsTemplate GetControlsTemplate()
		{
			IEnumerable<KeyValueTypesPairTemplate> modelControlPairTemplates = GetModelControlPairTemplates();
			IEnumerable<ControlTemplate> controlTemplates = GetControlTemplates();
			DefaultControlsTemplate defaultControlsTemplate = GetDefaultControlsTemplate();

			return new ControlsTemplate()
				.WithControlTemplates(controlTemplates)
				.WithDefaultControlsTemplate(defaultControlsTemplate)
				.WithModelControlPairTemplates(modelControlPairTemplates);
		}

		private DefaultControlsTemplate GetDefaultControlsTemplate()
		{
			IEnumerable<TypeIdentifier> requiredDefaulControlTypes = GetRequiredDefaultControlTypes();

			return new DefaultControlsTemplate()
				.WithRequiredDefaultControls(requiredDefaulControlTypes);
		}
		private IEnumerable<KeyValueTypesPairTemplate> GetModelControlPairTemplates()
		{
			IEnumerable<TypeIdentifier> requiredDefaulControlTypes = GetRequiredDefaultControlTypes();

			return requiredDefaulControlTypes.Select(GetModelControlPairTemplate);
		}
		private KeyValueTypesPairTemplate GetModelControlPairTemplate(TypeIdentifier requiredDefaultControlType)
		{
			return new KeyValueTypesPairTemplate()
				.WithValueType(DefaultControlsTemplate.DefaultModelControlPairs[requiredDefaultControlType])
				.WithKeyType(requiredDefaultControlType);
		}
		private IEnumerable<TypeIdentifier> GetRequiredDefaultControlTypes()
		{
			return DefaultControlsTemplate.DefaultModelControlPairs.Keys.Except(_modelSpace.FallbackControls.SelectMany(c => c.Models));
		}
		private IEnumerable<KeyValueTypesPairTemplate> GetModelControlPairTemplates(FallbackControl control)
		{
			return control.Models
					.Select(t => new KeyValueTypesPairTemplate()
						.WithKeyType(t)
						.WithValueType(control.Name));
		}

		private IEnumerable<ControlTemplate> GetControlTemplates()
		{
			IEnumerable<Model> modelsWithMissingControl = GetModelsWithMissingControlModel();

			var result = new List<ControlTemplate>();
			var exceptions = new List<Exception>();
			foreach (var model in modelsWithMissingControl)
			{
				try
				{
					result.Add(GetControlTemplate(model));
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
		private ControlTemplate GetControlTemplate(Model model)
		{
			ControlTypeIdentifierTemplate controlTypeIdentifierTemplate = GetControlTypeIdentifierTemplate(model);
			IEnumerable<SubControlTemplate> subControlTemplates = GetSubControlTemplates(model);

			return new ControlTemplate()
				.WithModel(model)
				.WithControlTypeIdentifierTemplate(controlTypeIdentifierTemplate)
				.WithSubControlTemplates(subControlTemplates);
		}
		private ControlTypeIdentifierTemplate GetControlTypeIdentifierTemplate(Model model)
		{
			return new ControlTypeIdentifierTemplate()
				.WithModelType(model.Name);
		}
		private IEnumerable<SubControlTemplate> GetSubControlTemplates(Model model)
		{
			var exceptions = new List<Exception>();
			var subControlTemplates = new List<SubControlTemplate>();
			foreach (var property in model.Properties)
			{
				try
				{
					subControlTemplates.Add(GetSubControlTemplate(model, property));
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);
				}
			}

			if (exceptions.Any())
			{
				var message = $"Error while generating control for {model.Name.ToEscapedString()}:\n{String.Join("\n", exceptions.Select(e => e.Message))}";
				throw new AggregateException(message, exceptions);
			}

			return subControlTemplates;
		}
		private SubControlTemplate GetSubControlTemplate(Model model, Property property)
		{
			if (property.Control == default)
			{
				throw new Exception($"Unable to locate control for {property.Name}. Make sure a control for {property.Type} is registered.");
			}

			var subControlPassAttributesTemplate = GetSubControlPassAttributesTemplate(model);

			return new SubControlTemplate()
				.WithModel(model)
				.WithProperty(property)
				.WithSubControlPassAttributesTemplate(subControlPassAttributesTemplate);
		}

		private SubControlPassAttributesTemplate GetSubControlPassAttributesTemplate(Model model)
		{
			return new SubControlPassAttributesTemplate()
				.WithAttributesProviderIdentifier(model.AttributesProvider);
		}

		private IEnumerable<Model> GetModelsWithMissingControlModel()
		{
			return _modelSpace.Models.Where(m => m.Control == default);
		}
		private FallbackControl GetControlModel(Model model)
		{
			ControlTypeIdentifierTemplate controlIdentifierTemplate = GetControlTypeIdentifierTemplate(model);
			TypeIdentifierName controlIdentifierName = TypeIdentifierName.Create().AppendNamePart(controlIdentifierTemplate.Build());
			TypeIdentifier controlIdentifier = TypeIdentifier.Create(controlIdentifierName, Namespace.Create());

			return FallbackControl.Create(controlIdentifier).Append(model.Name);
		}

		#endregion
	}
}
