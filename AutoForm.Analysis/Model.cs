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
		public readonly Property[] Properties;

		private Model(ITypeIdentifier name, ITypeIdentifier control, ITypeIdentifier template, ITypeIdentifier[] baseModels, Property[] properties)
		{
			properties.ThrowOnDuplicate(nameof(properties));

			Name = name;
			Control = control;
			Template = template;
			BaseModels = baseModels;
			Properties = properties ?? Array.Empty<Property>();
		}

		public static Model Create(ITypeIdentifier name, ITypeIdentifier[] baseModels)
		{
			return new Model(name, default, default, baseModels, Array.Empty<Property>());
		}
		public static Model Create(ITypeIdentifier name)
		{
			return Create(name, default);
		}

		public Model WithProperty(Property property)
		{
			return new Model(Name, Control, Template, BaseModels, Properties.Append(property).ToArray());
		}
		public Model WithControl(ITypeIdentifier control)
		{
			return new Model(Name, control, Template, BaseModels, Properties);
		}
		public Model WithTemplate(ITypeIdentifier template)
		{
			return new Model(Name, Control, template, BaseModels, Properties);
		}
		public Model WithProperties(IEnumerable<Property> properties)
		{
			return new Model(Name, Control, Template, BaseModels, Properties.Concat(properties).ToArray());
		}
	}
}
