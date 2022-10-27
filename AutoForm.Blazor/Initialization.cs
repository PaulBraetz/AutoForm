using AutoForm.Attributes;
using Fort;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoForm.Blazor
{
	public static class Initialization
	{
		public static Boolean TryInitialize(Assembly autoControlModelAssembly, out Exception? exception)
		{
			autoControlModelAssembly.ThrowIfDefault(nameof(autoControlModelAssembly));
			exception = null;

			try
			{
				RuntimeHelpers.RunClassConstructor(typeof(GeneratedControls).TypeHandle);
				var exceptions = new List<Exception>();

				//TODO: continue here

				var autoControls = autoControlModelAssembly
									.GetTypes()
									.Where(isModel)
									.Select(m => typeof(AutoControl<>).MakeGenericType(m));

				foreach (var autoControl in autoControls)
				{
					runClassConstructor(autoControl);
				}

				if (exceptions.Any())
				{
					throw new AggregateException(exceptions);
				}

				void runClassConstructor(Type t)
				{
					try
					{
						RuntimeHelpers.RunClassConstructor(t.TypeHandle);
					}
					catch (TypeInitializationException ex)
					{
						exceptions.Add(ex.InnerException!);
					}
					catch (Exception ex)
					{
						exceptions.Add(ex);
					}
				}

				Boolean isModel(Type type)
				{
					if (type == null)
					{
						return false;
					}

					var match = type != null &&
						(type.GetProperties()
							.Where(p => p.CustomAttributes.Any(a => a.GetType() == typeof(ModelPropertyAttribute)))
							.Any()/* ||
						isModel(type.BaseType)*/);

					return match;
				}
			}
			catch (TypeInitializationException ex)
			{
				exception = ex.InnerException;
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			return exception == null;
		}
	}
}
