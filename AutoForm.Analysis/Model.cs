using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
	internal readonly struct Model
	{
		public readonly ITypeIdentifier Name;
		public readonly ITypeIdentifier Control;
		public readonly ITypeIdentifier Template;
		public readonly ITypeIdentifier[] BaseModels;
		public readonly PropertyIdentifier[] BaseProperties;
		public readonly Property[] Properties;

		private Model(ITypeIdentifier name, ITypeIdentifier control, ITypeIdentifier template, ITypeIdentifier[] baseModels, Property[] properties, PropertyIdentifier[] baseProperties)
		{
			properties.ThrowOnDuplicate("property");
			baseProperties.ThrowOnDuplicate("base property");
			baseModels.ThrowOnDuplicate("base model");

			Name = name;
			Control = control;
			Template = template;
			BaseModels = baseModels;
			Properties = properties ?? Array.Empty<Property>();
			BaseProperties = baseProperties ?? Array.Empty<PropertyIdentifier>();
		}

		public static Model Create(ITypeIdentifier name, ITypeIdentifier[] baseModels, PropertyIdentifier[] baseProperties)
		{
			return new Model(name, default, default, baseModels, Array.Empty<Property>(), baseProperties);
		}
		public static Model Create(ITypeIdentifier name)
		{
			return Create(name, Array.Empty<ITypeIdentifier>(), Array.Empty<PropertyIdentifier>());
		}

		public Model AddProperty(Property property)
		{
			return new Model(Name, Control, Template, BaseModels, Properties.Append(property).ToArray(), BaseProperties);
		}
		public Model WithControl(ITypeIdentifier control)
		{
			return new Model(Name, control, Template, BaseModels, Properties, BaseProperties);
		}
		public Model WithTemplate(ITypeIdentifier template)
		{
			return new Model(Name, Control, template, BaseModels, Properties, BaseProperties);
		}
		[Obsolete("Use AddProperties instead.")]
		public Model WithProperties(IEnumerable<Property> properties)
		{
			return AddProperties(properties);
		}
		public Model AddProperties(IEnumerable<Property> properties)
		{
			return new Model(Name, Control, Template, BaseModels, Properties.Concat(properties).ToArray(), BaseProperties);
		}
		public Model RedefineProperties(IEnumerable<Property> properties)
		{
			return new Model(Name, Control, Template, BaseModels, properties.ToArray(), BaseProperties);
		}
		public Model AddBaseProperties(IEnumerable<PropertyIdentifier> baseProperties)
		{
			return new Model(Name, Control, Template, BaseModels, Properties, BaseProperties.Concat(baseProperties).ToArray());
		}
		public Model WithBaseProperties(IEnumerable<PropertyIdentifier> baseProperties)
		{
			return new Model(Name, Control, Template, BaseModels, Properties, baseProperties.ToArray());
		}
	}
}
