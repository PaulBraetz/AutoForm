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

		private Template(ITypeIdentifier name, ITypeIdentifier[] models)
		{
			models.ThrowOnDuplicate(Attributes.FallbackTemplate);

			Name = name;
			Models = models ?? Array.Empty<ITypeIdentifier>();
		}

		public static Template Create(ITypeIdentifier identifier)
		{
			return new Template(identifier, Array.Empty<ITypeIdentifier>());
		}
		public Template With(ITypeIdentifier modelIdentifier)
		{
			return new Template(Name, Models.Append(modelIdentifier).ToArray());
		}
		public Template WithRange(IEnumerable<ITypeIdentifier> modelIdentifiers)
		{
			return new Template(Name, Models.Concat(modelIdentifiers).ToArray());
		}
	}
}
