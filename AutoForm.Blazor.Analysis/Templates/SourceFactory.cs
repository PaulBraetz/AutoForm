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
                .WithMessage(String.Join("\n", _error.Exceptions));
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
            IEnumerable<String> requiredDefaulControlTypes = GetRequiredDefaultControlTypes();

            return new DefaultControlsTemplate()
                .WithRequiredDefaultControls(requiredDefaulControlTypes);
        }
        private IEnumerable<KeyValueTypesPairTemplate> GetModelControlPairTemplates()
        {
            IEnumerable<String> requiredDefaulControlTypes = GetRequiredDefaultControlTypes();

            return requiredDefaulControlTypes.Select(GetModelControlPairTemplate)
                .Concat(GetConcatenatedControlModels()
                    .SelectMany(GetModelControlPairTemplates));
        }
        private KeyValueTypesPairTemplate GetModelControlPairTemplate(String requiredDefaultControlType)
        {
            return new KeyValueTypesPairTemplate()
                .WithValueType(DefaultControlsTemplate.AvailableDefaultControls[requiredDefaultControlType])
                .WithKeyType(requiredDefaultControlType);
        }
        private IEnumerable<String> GetRequiredDefaultControlTypes()
        {
            return DefaultControlsTemplate.AvailableDefaultControls.Keys.Except(_modelSpace.FallbackControls.SelectMany(c => c.Models).Select(i => i.ToEscapedString()));
        }
        private IEnumerable<KeyValueTypesPairTemplate> GetModelControlPairTemplates(FallbackControl control)
        {
            return control.Models
                    .Select(t => new KeyValueTypesPairTemplate()
                        .WithKeyType(t.ToEscapedString())
                        .WithValueType(control.Name.ToEscapedString()));
        }

        private IEnumerable<ControlTemplate> GetControlTemplates()
        {
            IEnumerable<FallbackControl> allControlModels = GetConcatenatedControlModels();
            IEnumerable<Model> modelsWithMissingControl = GetModelModelsWithMissingControlModel();

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
        private ControlTemplate GetControlTemplate(Model model, IEnumerable<FallbackControl> controls)
        {
            var controlTypeIdentifierTemplate = GetControlTypeIdentifierTemplate(model);
            var subControlTemplates = GetSubControlTemplates(model, controls);
            var templateType = String.IsNullOrEmpty(model.Template.ToEscapedString()) ?
                _modelSpace.FallbackTemplates.SingleOrDefault(t => t.Models.Contains(model.Name)).Name.ToEscapedString() :
                model.Template.ToEscapedString();

            return new ControlTemplate()
                .WithModelType(model.Name.ToEscapedString())
                .WithControlTypeIdentifierTemplate(controlTypeIdentifierTemplate)
                .WithTemplateType(templateType)
                .WithSubControlTemplates(subControlTemplates);
        }
        private ControlTypeIdentifierTemplate GetControlTypeIdentifierTemplate(Model model)
        {
            return new ControlTypeIdentifierTemplate()
                .WithModelType(model.Name.ToEscapedString());
        }
        private IEnumerable<SubControlTemplate> GetSubControlTemplates(Model model, IEnumerable<FallbackControl> controls)
        {
            var exceptions = new List<Exception>();
            var subControlTemplates = new List<SubControlTemplate>();
            foreach (var property in model.Properties)
            {
                try
                {
                    subControlTemplates.Add(GetSubControlTemplate(model, property, controls));
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
        private SubControlTemplate GetSubControlTemplate(Model model, Property property, IEnumerable<FallbackControl> controls)
        {
            var controlType = String.IsNullOrEmpty(property.Control.ToEscapedString()) ? controls.SingleOrDefault(c => c.Models.Contains(property.Type)).Name.ToEscapedString() : property.Control.ToEscapedString();

            if (String.IsNullOrEmpty(controlType) && !DefaultControlsTemplate.AvailableDefaultControls.TryGetValue(property.Type.ToEscapedString(), out controlType))
            {
                throw new Exception($"Unable to locate control for {property.Name}. Make sure a control for {property.Type} is registered.");
            }

            var templateType = String.IsNullOrEmpty(property.Template.ToEscapedString()) ?
                _modelSpace.FallbackTemplates.SingleOrDefault(t => t.Models.Contains(property.Type)).Name.ToEscapedString() :
                property.Template.ToEscapedString();

            var subControlPassAttributesTemplate = GetSubControlPassAttributesTemplate(model);

            return new SubControlTemplate()
                .WithModelType(model.Name.ToEscapedString())
                .WithPropertyIdentifier(property.Name.ToEscapedString())
                .WithPropertyType(property.Type.ToEscapedString())
                .WithControlType(controlType)
                .WithTemplateType(templateType)
                .WithSubControlPassAttributesTemplate(subControlPassAttributesTemplate);
        }

        private SubControlPassAttributesTemplate GetSubControlPassAttributesTemplate(Model model)
        {
            return new SubControlPassAttributesTemplate()
                .WithAttributesProviderIdentifier(model.AttributesProvider.ToEscapedString());
        }

        private IEnumerable<FallbackControl> GetConcatenatedControlModels()
        {
            foreach (var control in _modelSpace.FallbackControls)
            {
                yield return control;
            }

            IEnumerable<Model> modelModelsWithMissingControlModels = GetModelModelsWithMissingControlModel();

            foreach (var model in modelModelsWithMissingControlModels)
            {
                yield return GetControlModel(model);
            }
        }
        private IEnumerable<Model> GetModelModelsWithMissingControlModel()
        {
            return _modelSpace.Models.Where(m => !_modelSpace.FallbackControls.Any(c => c.Models.Contains(m.Name)));
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
