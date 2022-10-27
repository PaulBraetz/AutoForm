using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoForm.Analysis
{
	internal readonly struct Template
	{
		public readonly ITypeIdentifier Name;
		public readonly ITypeIdentifier[] Models;
		public readonly PropertyIdentifier[] Properties;

		private Template(ITypeIdentifier name, ITypeIdentifier[] models, PropertyIdentifier[] properties)
		{
			models.ThrowOnDuplicate(Attributes.Control);
			properties.ThrowOnDuplicate(Attributes.Control);

			Name = name;
			Models = models ?? Array.Empty<ITypeIdentifier>();
			Properties = properties ?? Array.Empty<PropertyIdentifier>();
		}

		public static Template Create(ITypeIdentifier identifier)
		{
			return new Template(identifier, Array.Empty<ITypeIdentifier>(), Array.Empty<PropertyIdentifier>());
		}
		public Template With(ITypeIdentifier modelIdentifier)
		{
			return new Template(Name, Models.Append(modelIdentifier).ToArray(), Properties);
		}
		public Template WithRange(IEnumerable<ITypeIdentifier> modelIdentifiers)
		{
			return new Template(Name, Models.Concat(modelIdentifiers).ToArray(), Properties);
		}
		public Template With(PropertyIdentifier property)
		{
			return new Template(Name, Models, Properties.Append(property).ToArray());
		}
		public Template WithRange(IEnumerable<PropertyIdentifier> properties)
		{
			return new Template(Name, Models, Properties.Concat(properties).ToArray());
		}
	}
}
