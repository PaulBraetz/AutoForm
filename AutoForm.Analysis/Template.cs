﻿using System;
using System.Collections.Generic;
using System.Linq;

using RhoMicro.CodeAnalysis;

namespace AutoForm.Analysis
{
	internal readonly struct Template
	{
		public readonly ITypeIdentifier Name;
		public readonly ITypeIdentifier[] Models;
		public readonly PropertyIdentifier[] Properties;

		private Template(ITypeIdentifier name, ITypeIdentifier[] models, PropertyIdentifier[] properties)
		{
			models.ThrowOnDuplicate(Attributes.DefaultControl);
			properties.ThrowOnDuplicate(Attributes.DefaultControl);

			Name = name;
			Models = models ?? Array.Empty<ITypeIdentifier>();
			Properties = properties ?? Array.Empty<PropertyIdentifier>();
		}

		public static Template Create(ITypeIdentifier identifier) => new Template(identifier, Array.Empty<ITypeIdentifier>(), Array.Empty<PropertyIdentifier>());
		public Template WithModel(ITypeIdentifier model) => new Template(Name, Models.Append(model).ToArray(), Properties);
		public Template WithModels(IEnumerable<ITypeIdentifier> models) => new Template(Name, Models.Concat(models).ToArray(), Properties);
		public Template WithProperty(PropertyIdentifier property) => new Template(Name, Models, Properties.Append(property).ToArray());
		public Template WithProperties(IEnumerable<PropertyIdentifier> properties) => new Template(Name, Models, Properties.Concat(properties).ToArray());
	}
}
