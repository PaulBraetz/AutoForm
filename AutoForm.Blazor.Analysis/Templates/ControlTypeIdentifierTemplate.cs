using AutoForm.Analysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoForm.Generate.Blazor.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct ControlTypeIdentifierTemplate
		{
			private sealed class EqualityComparer : IEqualityComparer<ControlTypeIdentifierTemplate>
			{
				private EqualityComparer()
				{

				}

				public static readonly EqualityComparer Instance = new EqualityComparer();

				public Boolean Equals(ControlTypeIdentifierTemplate x, ControlTypeIdentifierTemplate y)
				{
					return x._identifier == y._identifier;
				}

				public Int32 GetHashCode(ControlTypeIdentifierTemplate obj)
				{
					return EqualityComparer<String>.Default.GetHashCode(obj._identifier);
				}
			}

			private ControlTypeIdentifierTemplate(String identifier)
			{
				_identifier = identifier;
			}

			private const String TEMPLATE = "__Control_" + MODEL_TYPE;

			private readonly String _identifier;

			public ControlTypeIdentifierTemplate WithModelType(TypeIdentifier modelType)
			{
				return new ControlTypeIdentifierTemplate(modelType.ToEscapedString().Replace('.', '_'));
			}
			private ControlTypeIdentifierTemplate WithIdentifier(String identifier)
			{
				return new ControlTypeIdentifierTemplate(identifier);
			}

			public static String Sanitize(String built)
			{
				IEnumerable<ControlTypeIdentifierTemplate> invalidIdentifiers = Regex.Matches(built, @"(?<=__Control_).*__")
					.OfType<Match>()
					.Select(m => m.Value)
					.Select(new ControlTypeIdentifierTemplate().WithIdentifier)
					.Distinct(EqualityComparer.Instance);

				Int32 index = 0;
				foreach (var invalidIdentifier in invalidIdentifiers)
				{
					built = built
						.Replace(invalidIdentifier.Build(), new ControlTypeIdentifierTemplate().WithIdentifier(index.ToString()).Build());
					index++;
				}

				return built;
			}

			public String Build()
			{
				return TEMPLATE
					.Replace(MODEL_TYPE, _identifier);
			}

			public override String ToString()
			{
				return Build();
			}
		}
	}
}