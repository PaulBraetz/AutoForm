using System;
using System.Collections.Generic;
using System.Linq;

using RhoMicro.CodeAnalysis;

namespace AutoForm.Analysis
{
	internal readonly struct Control
	{
		public readonly ITypeIdentifier Name;
		public readonly ITypeIdentifier[] Models;
		public readonly PropertyIdentifier[] Properties;

		private Control(ITypeIdentifier name, ITypeIdentifier[] models, PropertyIdentifier[] properties)
		{
			models.ThrowOnDuplicate(Attributes.DefaultControl);
			properties.ThrowOnDuplicate(Attributes.DefaultControl);

			Name = name;
			Models = models ?? Array.Empty<ITypeIdentifier>();
			Properties = properties ?? Array.Empty<PropertyIdentifier>();
		}

		public static Control CreateGenerated(ITypeIdentifier modelType)
		{
			var identifier = modelType.AsGenerated();

			var control = Create(identifier)
				.WithModel(modelType);

			return control;
		}

		public static Control Create(ITypeIdentifier model) => new Control(model, Array.Empty<ITypeIdentifier>(), Array.Empty<PropertyIdentifier>());
		public Control WithModel(ITypeIdentifier modelIdentifier) => new Control(Name, Models.Append(modelIdentifier).ToArray(), Properties);
		public Control WithModels(IEnumerable<ITypeIdentifier> models) => new Control(Name, Models.Concat(models).ToArray(), Properties);
		public Control WithProperty(PropertyIdentifier property) => new Control(Name, Models, Properties.Append(property).ToArray());
		public Control WithProperties(IEnumerable<PropertyIdentifier> properties) => new Control(Name, Models, Properties.Concat(properties).ToArray());
	}
}
