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
				IEnumerable<Control> defaultFallbackControls = DefaultControlsTemplate.DefaultModelControlPairs
					.Where(kvp => !modelSpace.FallbackControls.SelectMany(c => c.Models).Contains(kvp.Key))
					.GroupBy(kvp => kvp.Value)
					.Select(group => Control.Create(group.Key).WithRange(group.Select(kvp => kvp.Key)));

				modelSpace = modelSpace.WithFallbackControls(defaultFallbackControls).WithRequiredGeneratedControls();
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
			IEnumerable<KeyValueTypesPairTemplate> modelTemplatePairTemplates = GetModelTemplatePairTemplates();
			IEnumerable<ControlTemplate> controlTemplates = GetControlTemplates();
			DefaultControlsTemplate defaultControlsTemplate = GetDefaultControlsTemplate();

			return new ControlsTemplate()
				.WithControlTemplates(controlTemplates)
				.WithDefaultControlsTemplate(defaultControlsTemplate)
				.WithModelControlPairTemplates(modelControlPairTemplates)
				.WithModelTemplatePairTemplates(modelTemplatePairTemplates);
		}

		private IEnumerable<KeyValueTypesPairTemplate> GetModelTemplatePairTemplates()
		{
			return _modelSpace.FallbackTemplates.SelectMany(t => t.Models.Select(m => new KeyValueTypesPairTemplate().WithKeyType(m).WithValueType(t.Name)));
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

			return GetControlsToBeGenerated().SelectMany(c => c.Models.Select(m => GetModelControlPairTemplate(m, c.Name)))//)
				.Concat(_modelSpace.FallbackControls.SelectMany(c => c.Models.Select(m => GetModelControlPairTemplate(m, c.Name))));
		}
		private KeyValueTypesPairTemplate GetModelControlPairTemplate(TypeIdentifier requiredDefaultControlType)
		{
			return GetModelControlPairTemplate(DefaultControlsTemplate.DefaultModelControlPairs[requiredDefaultControlType], requiredDefaultControlType);
		}
		private KeyValueTypesPairTemplate GetModelControlPairTemplate(TypeIdentifier key, TypeIdentifier value)
		{
			return new KeyValueTypesPairTemplate()
				.WithValueType(value)
				.WithKeyType(key);
		}
		private IEnumerable<TypeIdentifier> GetRequiredDefaultControlTypes()
		{
			return DefaultControlsTemplate.DefaultModelControlPairs.Keys
				.Except(GetControlsToBeGenerated().SelectMany(c => c.Models));
		}

		private IEnumerable<Control> GetControlsToBeGenerated()
		{
			return _modelSpace.RequiredGeneratedControls.Where(c => !DefaultControlsTemplate.DefaultModelControlPairs.Values.Contains(c.Name));
		}

		private IEnumerable<KeyValueTypesPairTemplate> GetModelControlPairTemplates(Control control)
		{
			return control.Models
					.Select(t => new KeyValueTypesPairTemplate()
						.WithKeyType(t)
						.WithValueType(control.Name));
		}

		private IEnumerable<ControlTemplate> GetControlTemplates()
		{
			var result = new List<ControlTemplate>();
			var exceptions = new List<Exception>();
			var defaults = DefaultControlsTemplate.DefaultModelControlPairs.Select(kvp => kvp.Value);
			foreach (var requiredControl in _modelSpace.RequiredGeneratedControls.Where(c => !defaults.Contains(c.Name)))
			{
				try
				{
					result.Add(GetControlTemplate(requiredControl));
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
		private ControlTemplate GetControlTemplate(Control requiredControl)
		{
			var model = _modelSpace.Models.Single(m => requiredControl.Models.Contains(m.Name));

			IEnumerable<SubControlTemplate> subControlTemplates = GetSubControlTemplates(model);

			return new ControlTemplate()
				.WithModelType(model.Name)
				.WithControlType(requiredControl.Name)
				.WithSubControlTemplates(subControlTemplates);
		}

		private IEnumerable<SubControlTemplate> GetSubControlTemplates(Model model)
		{
			var exceptions = new List<Exception>();
			var subControlTemplates = new List<SubControlTemplate>();
			var properties = model.Properties.OrderBy(p => p.Order);
			foreach (var property in properties)
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

		#endregion
	}
}
