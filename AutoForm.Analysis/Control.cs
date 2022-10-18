
/* Unmerged change from project 'AutoForm.Json.Analysis'
Before:
using System;
After:
using AutoForm.Analysis;
*/
using RhoMicro.CodeAnalysis;
using RhoMicro.CodeAnalysis;

/* Unmerged change from project 'AutoForm.Json.Analysis'
Before:
using RhoMicro.CodeAnalysis;
After:
using System;
*/
/* Unmerged change from project 'AutoForm.Json.Analysis'
Before:
using AutoForm.Analysis;
using RhoMicro.CodeAnalysis;
After:
using System.Collections.Generic;
using System.Linq;
*/

namespace AutoForm.Analysis
{
	internal readonly struct Control
	{
		public readonly ITypeIdentifier Name;
		public readonly ITypeIdentifier[] Models;

		private Control(ITypeIdentifier name, ITypeIdentifier[] models)
		{
			models.ThrowOnDuplicate(Attributes.FallbackControl);

			Name = name;
			Models = models ?? Array.Empty<ITypeIdentifier>();
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
			return new Control(identifier, Array.Empty<ITypeIdentifier>());
		}
		public Control With(ITypeIdentifier modelIdentifier)
		{
			return new Control(Name, Models.Append(modelIdentifier).ToArray());
		}
		public Control WithRange(IEnumerable<ITypeIdentifier> modelIdentifiers)
		{
			return new Control(Name, Models.Concat(modelIdentifiers).ToArray());
		}
	}
}
