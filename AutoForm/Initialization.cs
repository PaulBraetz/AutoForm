using Fort;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoForm
{
    public static class Initialization
    {
        public static Boolean TryInitialize(Assembly autoControlModelAssembly, out Exception? exception)
        {
            autoControlModelAssembly.ThrowIfDefault(nameof(autoControlModelAssembly));
            exception = null;

            try
            {
                RuntimeHelpers.RunClassConstructor(typeof(Controls).TypeHandle);
                var exceptions = new List<Exception>();

                var autoControls = autoControlModelAssembly
                                    .GetTypes()
                                    .Where(t => t.CustomAttributes.Any(a => a.AttributeType == typeof(AutoControlModelAttribute)))
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
            }
            catch(TypeInitializationException ex)
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
