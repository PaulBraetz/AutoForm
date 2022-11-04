using RhoMicro.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace AutoForm.Blazor.Analysis.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct ErrorTemplate
		{
			private ErrorTemplate(Error error)
			{
				_error = error;
			}

			private readonly Error _error;

			private const String TEMPLATE =
	@"//One or more errors have occured during generation of this source. See the information below:
/*
" + ERROR_MESSAGE + @"
*/";

			public ErrorTemplate WithError(Error error)
			{
				return new ErrorTemplate(error);
			}

			public String Build()
			{
				var errorMessage = String.Join("\n\n", (IEnumerable<Exception>)_error.Exceptions);

				return TEMPLATE
					.Replace(ERROR_MESSAGE, errorMessage);
			}

			public override String ToString()
			{
				return Build();
			}
		}
	}
}