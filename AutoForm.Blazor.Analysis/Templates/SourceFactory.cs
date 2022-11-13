using AutoForm.Analysis;
using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoForm.Blazor.Analysis.Templates
{
	internal sealed partial class SourceFactory
	{
		#region Placeholders
		private const String MODEL_CONTROL_PAIRS = "{" + nameof(MODEL_CONTROL_PAIRS) + "}";
		private const String MODEL_TEMPLATE_PAIRS = "{" + nameof(MODEL_TEMPLATE_PAIRS) + "}";
		private const String CONTROLS = "{" + nameof(CONTROLS) + "}";

		private const String CONTROL_TYPE = "{" + nameof(CONTROL_TYPE) + "}";
		private const String CONTROL_TYPE_IDENTIFIER = "{" + nameof(CONTROL_TYPE_IDENTIFIER) + "}";

		private const String TEMPLATE_TYPE = "{" + nameof(TEMPLATE_TYPE) + "}";

		private const String LINE_INDEX = "{" + nameof(LINE_INDEX) + "}";

		private const String MODEL_TYPE = "{" + nameof(MODEL_TYPE) + "}";

		private const String SUB_CONTROLS = "{" + nameof(SUB_CONTROLS) + "}";

		private const String PROPERTY_TYPE = "{" + nameof(PROPERTY_TYPE) + "}";
		private const String PROPERTY_IDENTIFIER = "{" + nameof(PROPERTY_IDENTIFIER) + "}";

		private const String ATTRIBUTES_PROVIDER_IDENTIFIER = "{" + nameof(ATTRIBUTES_PROVIDER_IDENTIFIER) + "}";

		private const String ERROR_MESSAGE = "{" + nameof(ERROR_MESSAGE) + "}";
		#endregion

		#region Available Default Controls
		private static readonly Namespace ControlsNamespace = Namespace.Create().AppendRange(new[] { "AutoForm", "Blazor", "Controls" });
		private static readonly IReadOnlyDictionary<ITypeIdentifier, ITypeIdentifier> AvailableDefaultControls = new ReadOnlyDictionary<ITypeIdentifier, ITypeIdentifier>(new Dictionary<ITypeIdentifier, ITypeIdentifier>()
		{
			{TypeIdentifier.Create<String>(), GetDefaultControl("Text") },
			{TypeIdentifier.Create<Char>(), GetDefaultControl("CharControl") },
			{TypeIdentifier.Create<DateTime>(), GetDefaultControl("DateControl") },
			{TypeIdentifier.Create<Boolean>(), GetDefaultControl("Checkbox") },
			{TypeIdentifier.Create<Byte>(), GetDefaultControl("ByteNumber") },
			{TypeIdentifier.Create<SByte>(), GetDefaultControl("SByteNumber") },
			{TypeIdentifier.Create<Int16>(), GetDefaultControl("Int16Number") },
			{TypeIdentifier.Create<UInt16>(), GetDefaultControl("UInt16Number") },
			{TypeIdentifier.Create<Int32>(), GetDefaultControl("Int32Number") },
			{TypeIdentifier.Create<UInt32>(), GetDefaultControl("UInt32Number") },
			{TypeIdentifier.Create<Int64>(), GetDefaultControl("Int64Number") },
			{TypeIdentifier.Create<UInt64>(), GetDefaultControl("UInt64Number") },
			{TypeIdentifier.Create<Single>(), GetDefaultControl("SingleNumber") },
			{TypeIdentifier.Create<Double>(), GetDefaultControl("DoubleNumber") },
			{TypeIdentifier.Create<Decimal>(), GetDefaultControl("DecimalNumber") }
		});
		private static ITypeIdentifier GetDefaultControl(String name)
		{
			return TypeIdentifier.Create(TypeIdentifierName.Create().AppendNamePart(name), ControlsNamespace);
		}
		#endregion

		private readonly ModelSpace _modelSpace;
		private readonly Error _error;
		private readonly Boolean _isError;

		private SourceFactory(ModelSpace modelSpace, Error error, Boolean isError)
		{
			_modelSpace = modelSpace;
			_error = error;
			_isError = isError;
		}
		private SourceFactory(ModelSpace modelSpace) : this(modelSpace, default, false)
		{
			var defaultFallbackControls = AvailableDefaultControls
				.Where(kvp => !_modelSpace.Controls.SelectMany(c => c.Models).Contains(kvp.Key))
				.GroupBy(kvp => kvp.Value)
				.Select(group => Control.Create(group.Key).WithModels(group.Select(kvp => kvp.Key)));

			_modelSpace = _modelSpace.WithFallbackControls(defaultFallbackControls).WithRequiredGeneratedControls();
		}
		private SourceFactory(Error error) : this(default, error, true)
		{
		}

		public static SourceFactory Create(ModelSpace modelSpace)
		{
			return new SourceFactory(modelSpace);
		}
		public static SourceFactory Create(Error error)
		{
			return new SourceFactory(error);
		}

		public String Build()
		{
			var result = _isError ?
				GetError() :
				GetControls();

			return result;
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
			var modelControlPairTemplates = GetModelControlPairTemplates();
			var modelTemplatePairTemplates = GetModelTemplatePairTemplates();
			var controlTemplates = GetControlTemplates();

			return new ControlsTemplate()
				.WithControlTemplates(controlTemplates)
				.WithModelControlPairTemplates(modelControlPairTemplates)
				.WithModelTemplatePairTemplates(modelTemplatePairTemplates);
		}

		private IEnumerable<KeyValueTypesPairTemplate> GetModelTemplatePairTemplates()
		{
			return _modelSpace.Templates.SelectMany(t => t.Models.Select(m => new KeyValueTypesPairTemplate().WithKeyType(m).WithValueType(t.Name)));
		}
		private IEnumerable<KeyValueTypesPairTemplate> GetModelControlPairTemplates()
		{
			var requiredDefaulControlTypes = GetRequiredDefaultControlTypes();

			return GetControlsToBeGenerated().SelectMany(c => c.Models.Select(m => GetModelControlPairTemplate(m, c.Name)))//)
				.Concat(_modelSpace.Controls.SelectMany(c => c.Models.Select(m => GetModelControlPairTemplate(m, c.Name))));
		}
		private KeyValueTypesPairTemplate GetModelControlPairTemplate(ITypeIdentifier key, ITypeIdentifier value)
		{
			return new KeyValueTypesPairTemplate()
				.WithValueType(value)
				.WithKeyType(key);
		}
		private IEnumerable<ITypeIdentifier> GetRequiredDefaultControlTypes()
		{
			return AvailableDefaultControls.Keys
				.Except(GetControlsToBeGenerated().SelectMany(c => c.Models));
		}

		private IEnumerable<Control> GetControlsToBeGenerated()
		{
			return _modelSpace.RequiredGeneratedControls.Where(c => !AvailableDefaultControls.Values.Contains(c.Name));
		}

		private IEnumerable<ControlTemplate> GetControlTemplates()
		{
			var result = new List<ControlTemplate>();
			var exceptions = new List<Exception>();
			var defaults = AvailableDefaultControls.Select(kvp => kvp.Value);
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

			var subControlTemplates = GetSubControlTemplates(model);

			return new ControlTemplate()
				.WithModelType(model.Name)
				.WithControlType(requiredControl.Name)
				.WithSubControlTemplates(subControlTemplates);
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
				var message = $"Error while generating control for {model.Name}:\n{String.Join("\n", exceptions.Select(e => e.Message))}";
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

			return new SubControlTemplate()
				.WithModel(model)
				.WithProperty(property);
		}
		#endregion
	}
}
