using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoForm
{
	public static class Initialization
	{
		public static Boolean TryInitialize(out Exception? exception)
		{
			exception = null;
			try
			{
				RuntimeHelpers.RunClassConstructor(typeof(AutoControlBase).TypeHandle);
			}
			catch (TypeInitializationException ex)
			{
				exception = FilterTypeInitializationException(ex);
			}
			return exception == null;
		}

		private static Exception? FilterTypeInitializationException(Exception? exception)
		{
			if (exception is TypeInitializationException)
			{
				return FilterTypeInitializationException(exception.InnerException);
			}
			if (exception is AggregateException aggregateException)
			{
				return new AggregateException(aggregateException.InnerExceptions.Select(FilterTypeInitializationException)!);
			}
			return exception;
		}
	}
}
