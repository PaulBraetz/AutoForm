using System.Reflection;

namespace AutoForm.Blazor
{
    public static class Controls
    {
        static Controls()
        {
            Type? type = Assembly.GetEntryAssembly()?
                .GetType("AutoForm.Blazor.GeneratedControls");
            if (type == null)
            {
                throw new Exception("Unable to locate AutoForm.Blazor.GeneratedControls. Make sure that the AutoForm.Blazor.Analysis.BlazorGenerator has run.");
            }

            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            FieldInfo? field = type?
                .GetField("ModelControlMap");

            if (field == null)
            {
                throw new Exception("Unable to locate ModelControlMap in AutoForm.Blazor.GeneratedControls. Make sure that the AutoForm.Blazor.Analysis.BlazorGenerator has run.");
            }

            ModelControlMap = (IDictionary<Type, Type>)field.GetValue(null)!;
        }

        public static IDictionary<Type, Type> ModelControlMap { get; private set; }
    }
}
