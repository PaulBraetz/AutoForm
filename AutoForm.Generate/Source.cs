using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		#region Placeholders
		private const String MODEL_CONTROL_PAIRS = "{" + nameof(MODEL_CONTROL_PAIRS) + "}";
		private const String CONTROLS = "{" + nameof(CONTROLS) + "}";

		private const String CONTROL_INDEX = "{" + nameof(CONTROL_INDEX) + "}";
		private const String CONTROL_TYPE = "{" + nameof(CONTROL_TYPE) + "}";
		private const String CONTROL_TYPE_IDENTIFIER_TEMPLATE = "{" + nameof(CONTROL_TYPE_IDENTIFIER_TEMPLATE) + "}";

		private const String SUB_CONTROL_TYPE_FIELDS = "{" + nameof(SUB_CONTROL_TYPE_FIELDS) + "}";
		private const String SUB_CONTROL_TYPE_FIELD_INDEX = "{" + nameof(SUB_CONTROL_TYPE_FIELD_INDEX) + "}";
		private const String SUB_CONTROL_LINE_INDEX = "{" + nameof(SUB_CONTROL_LINE_INDEX) + "}";
		private const String SUB_CONTROL_TYPE = "{" + nameof(SUB_CONTROL_TYPE) + "}";
		private const String SUB_CONTROL_TYPE_FIELD_IDENTIFIER_TEMPLATE = "{" + nameof(SUB_CONTROL_TYPE_FIELD_IDENTIFIER_TEMPLATE) + "}";

		private const String MODEL_TYPE = "{" + nameof(MODEL_TYPE) + "}";

		private const String SUB_CONTROLS = "{" + nameof(SUB_CONTROLS) + "}";

		private const String PROPERTY_TYPE = "{" + nameof(PROPERTY_TYPE) + "}";
		private const String PROPERTY_IDENTIFIER = "{" + nameof(PROPERTY_IDENTIFIER) + "}";

		#endregion

		#region Models
		public readonly struct ControlModel : IEquatable<ControlModel>
		{
			public readonly String ControlType;
			public readonly String ModelType;

			public ControlModel(String controlType, String modelType)
			{
				ControlType = controlType;
				ModelType = modelType;
			}

			public override Boolean Equals(Object obj)
			{
				return obj is ControlModel model && Equals(model);
			}

			public Boolean Equals(ControlModel other)
			{
				return ControlType == other.ControlType &&
					   ModelType == other.ModelType;
			}

			public override Int32 GetHashCode()
			{
				Int32 hashCode = -1719137624;
				hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(ControlType);
				hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(ModelType);
				return hashCode;
			}

			public static Boolean operator ==(ControlModel left, ControlModel right)
			{
				return left.Equals(right);
			}

			public static Boolean operator !=(ControlModel left, ControlModel right)
			{
				return !(left == right);
			}
		}
		public readonly struct ModelModel : IEquatable<ModelModel>
		{
			public readonly String ModelType;
			public readonly IEnumerable<PropertyModel> Properties;

			public ModelModel(String modelType, IEnumerable<PropertyModel> properties)
			{
				ModelType = modelType;
				Properties = properties;
			}

			public override Boolean Equals(Object obj)
			{
				return obj is ModelModel model && Equals(model);
			}

			public Boolean Equals(ModelModel other)
			{
				return ModelType == other.ModelType &&
					   EqualityComparer<IEnumerable<PropertyModel>>.Default.Equals(Properties, other.Properties);
			}

			public override Int32 GetHashCode()
			{
				Int32 hashCode = 1934847142;
				hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(ModelType);
				hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<PropertyModel>>.Default.GetHashCode(Properties);
				return hashCode;
			}

			public static Boolean operator ==(ModelModel left, ModelModel right)
			{
				return left.Equals(right);
			}

			public static Boolean operator !=(ModelModel left, ModelModel right)
			{
				return !(left == right);
			}
		}
		public readonly struct PropertyModel : IEquatable<PropertyModel>
		{
			public readonly String PropertyType;
			public readonly String PropertyIdentifier;

			public PropertyModel(String propertyIdentifier, String propertyType)
			{
				PropertyIdentifier = propertyIdentifier;
				PropertyType = propertyType;
			}

			public override Boolean Equals(Object obj)
			{
				return obj is PropertyModel model && Equals(model);
			}

			public Boolean Equals(PropertyModel other)
			{
				return PropertyType == other.PropertyType &&
					   PropertyIdentifier == other.PropertyIdentifier;
			}

			public override Int32 GetHashCode()
			{
				Int32 hashCode = 1748401947;
				hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(PropertyType);
				hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(PropertyIdentifier);
				return hashCode;
			}

			public static Boolean operator ==(PropertyModel left, PropertyModel right)
			{
				return left.Equals(right);
			}

			public static Boolean operator !=(PropertyModel left, PropertyModel right)
			{
				return !(left == right);
			}
		}
		#endregion

		public static String GetControls(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			return GetControlsTemplate(models, controls).Build();
		}

		private static ControlsTemplate GetControlsTemplate(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			IEnumerable<ModelControlPairTemplate> modelControlPairTemplates = GetModelControlPairTemplates(models, controls);
			IEnumerable<ControlTemplate> controlTemplates = GetControlTemplates(models, controls);

			return new ControlsTemplate()
				.WithControlTemplates(controlTemplates)
				.WithModelControlPairTemplates(modelControlPairTemplates);
		}

		private static IEnumerable<ModelControlPairTemplate> GetModelControlPairTemplates(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			Int32 controlIndex = 0;
			var modelControlPairTemplates = controls
				.Select(GetModelControlPairTemplate)
				.Concat(models
					.Where(m => !controls.Any(c => c.ModelType == m.ModelType))
					.Select(m => GetModelControlPairTemplate(ref controlIndex, m)));
			return modelControlPairTemplates;
		}

		private static IEnumerable<ControlTemplate> GetControlTemplates(IEnumerable<ModelModel> models, IEnumerable<ControlModel> controls)
		{
			Int32 controlIndex = 0;
			return models
				.Where(m => !controls.Any(c => c.ModelType == m.ModelType))
				.Select(m => GetControlTemplate(ref controlIndex, m));
		}

		private static ModelControlPairTemplate GetModelControlPairTemplate(ref Int32 controlIndex, ModelModel model)
		{
			return new ModelControlPairTemplate()
						.WithModelType(model.ModelType)
						.WithControlType(new ControlTypeIdentifierTemplate().Build(ref controlIndex));
		}

		private static ModelControlPairTemplate GetModelControlPairTemplate(ControlModel control)
		{
			return new ModelControlPairTemplate()
					.WithModelType(control.ModelType)
					.WithControlType(control.ControlType);
		}

		private static ControlTemplate GetControlTemplate(ref Int32 controlIndex, ModelModel model)
		{
			var subControlTypeFieldTemplates = GetSubControlTypeFieldTemplates(model);
			var controlTypeIdentifierTemplate = GetControlTypeIdentifierTemplate();
			var subControlTemplates = GetSubControlTemplates(model);

			return new ControlTemplate()
				.WithModelType(model.ModelType)
				.WithSubControlTypeFieldTemplates(subControlTypeFieldTemplates)
				.WithControlTypeIdentifierTemplate(controlTypeIdentifierTemplate)
				.WithSubControlTemplates(subControlTemplates);
		}

		private static IEnumerable<SubControlTypeFieldTemplate> GetSubControlTypeFieldTemplates(ModelModel model)
		{
			return model.Properties.Select(p => new SubControlTypeFieldTemplate()
				.WithModelType(model.ModelType)
				.WithPropertyIdentifier(p.PropertyIdentifier)
				.WithPropertyType(p.PropertyType)
				.WithSubControlFieldIdentifierTemplate(GetSubControlFieldIdentifierTemplate()));
		}

		private static SubControlFieldIdentifierTemplate GetSubControlFieldIdentifierTemplate()
		{
			return new SubControlFieldIdentifierTemplate();
		}

		private static ControlTypeIdentifierTemplate GetControlTypeIdentifierTemplate()
		{
			return new ControlTypeIdentifierTemplate();
		}

		private static IEnumerable<SubControlTemplate> GetSubControlTemplates(ModelModel model)
		{
			return model.Properties.Select(p => new SubControlTemplate()
				.WithModelType(model.ModelType)
				.WithPropertyIdentifier(p.PropertyIdentifier)
				.WithPropertyType(p.PropertyType)
				.WithSubControlFieldIdentifierTemplate(GetSubControlFieldIdentifierTemplate()));
		}
	}
}
