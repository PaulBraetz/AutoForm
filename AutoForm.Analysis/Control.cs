using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutoForm.Analysis
{
	internal readonly struct Control
	{
		public readonly ITypeIdentifier Name;
		public readonly ITypeIdentifier[] Models;
		public readonly PropertyIdentifier[] Properties;

		private Control(ITypeIdentifier name, ITypeIdentifier[] models, PropertyIdentifier[] properties)
		{
			models.ThrowOnDuplicate(Attributes.Control);
			properties.ThrowOnDuplicate(Attributes.Control);

			Name = name;
			Models = models ?? Array.Empty<ITypeIdentifier>();
			Properties = properties ?? Array.Empty<PropertyIdentifier>();
		}

		public static Control CreateGenerated(ITypeIdentifier modelType)
		{
			var identifier = modelType.AsGenerated();

			var control = Create(identifier)
				.With(modelType);

			return control;
		}

		public static Control Create(ITypeIdentifier identifier)
		{
			return new Control(identifier, Array.Empty<ITypeIdentifier>(), Array.Empty<PropertyIdentifier>());
		}
		public Control With(ITypeIdentifier modelIdentifier)
		{
			return new Control(Name, Models.Append(modelIdentifier).ToArray(), Properties);
		}
		public Control WithRange(IEnumerable<ITypeIdentifier> modelIdentifiers)
		{
			return new Control(Name, Models.Concat(modelIdentifiers).ToArray(), Properties);
		}
		public Control With(PropertyIdentifier property)
		{
			return new Control(Name, Models, Properties.Append(property).ToArray());
		}
		public Control WithRange(IEnumerable<PropertyIdentifier> properties)
		{
			return new Control(Name, Models, Properties.Concat(properties).ToArray());
		}
	}
}
