using AutoForm.Generate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoForm.Generate.Blazor.Templates
{
    internal readonly partial struct SourceFactory
    {
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
            IEnumerable<ModelControlPairTemplate> modelControlPairTemplates = GetModelControlPairTemplates();
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
        private IEnumerable<ModelControlPairTemplate> GetModelControlPairTemplates()
        {
            IEnumerable<String> requiredDefaulControlTypes = GetRequiredDefaultControlTypes();

            return requiredDefaulControlTypes.Select(GetModelControlPairTemplate)
                .Concat(GetConcatenatedControlModels()
                .SelectMany(GetModelControlPairTemplates));
        }
        private ModelControlPairTemplate GetModelControlPairTemplate(String requiredDefaultControlType)
        {
            return new ModelControlPairTemplate()
                .WithControlType(DefaultControlsTemplate.AvailableDefaultControls[requiredDefaultControlType])
                .WithModelType(requiredDefaultControlType);
        }
        private IEnumerable<String> GetRequiredDefaultControlTypes()
        {
            return DefaultControlsTemplate.AvailableDefaultControls.Keys.Except(_modelSpace.Controls.Select(c => c.ModelIdentifier.ToString()));
        }
        private IEnumerable<ModelControlPairTemplate> GetModelControlPairTemplates(Control control)
        {
            return control.ModelIdentifier
                    .Select(t => new ModelControlPairTemplate()
                        .WithModelType(t)
                        .WithControlType(control.Type));
        }

        private IEnumerable<ControlTemplate> GetControlTemplates(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
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
        private ControlTemplate GetControlTemplate(ModelModel model, IEnumerable<ControlModel> controls)
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
        private IEnumerable<SubControlPropertyTemplate> GetSubControlPropertyTemplates(ModelModel model)
        {
            return model.Properties.Select(GetSubControlPropertyTemplate);
        }
        private SubControlPropertyTemplate GetSubControlPropertyTemplate(PropertyModel property)
        {
            SubControlPropertyIdentifierTemplate subControlPropertyIdentifierTemplate = GetSubControlPropertyIdentifierTemplate(property);

            return new SubControlPropertyTemplate()
                .WithSubControlPropertyIdentifierTemplate(subControlPropertyIdentifierTemplate)
                .WithPropertyIdentifier(property.Identifier)
                .WithPropertyType(property.Type);
        }
        private ControlTypeIdentifierTemplate GetControlTypeIdentifierTemplate(ModelModel model)
        {
            return new ControlTypeIdentifierTemplate()
                .WithModelType(model.Type);
        }
        private IEnumerable<SubControlTemplate> GetSubControlTemplates(ModelModel model, IEnumerable<ControlModel> controls)
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
        private SubControlTemplate GetSubControlTemplate(ModelModel model, PropertyModel property, IEnumerable<ControlModel> controls)
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

        private SubControlPassAttributesTemplate GetSubControlPassAttributesTemplate(ModelModel model)
        {
            return new SubControlPassAttributesTemplate()
                .WithAttributesProviderIdentifier(model.AttributesProviderIdentifier);
        }

        private SubControlPropertyIdentifierTemplate GetSubControlPropertyIdentifierTemplate(PropertyModel property)
        {
            return new SubControlPropertyIdentifierTemplate()
                .WithPropertyIdentifier(property.Identifier);
        }

        private IEnumerable<ControlModel> GetConcatenatedControlModels(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
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
        private IEnumerable<ModelModel> GetModelModelsWithMissingControlModel(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
        {
            return models.Where(m => !controls.Any(c => c.ModelTypes.Contains(m.Type)));
        }
        private ControlModel GetControlModel(ModelModel model)
        {
            ControlTypeIdentifierTemplate controlIdentifierTemplate = GetControlTypeIdentifierTemplate(model);
            return new ControlModel(controlIdentifierTemplate.Build(), new[] { model.Type });
        }

        #endregion
    }
}
