using System.Reflection;

namespace AutoForm.Blazor
{
    public static class GeneratedControls
    {
        static GeneratedControls()
        {
            var type = Assembly.GetEntryAssembly()?
                .GetType("AutoForm.Blazor.GeneratedControls");
            if (type == null)
            {
                throw new Exception("Unable to locate AutoForm.Blazor.GeneratedControls. Make sure that the AutoForm.Blazor.Analysis.BlazorGenerator has run.");
            }

            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            var modelControlMapField = type?
                .GetField("ModelControlMap");

            if (modelControlMapField == null)
            {
                throw new Exception("Unable to locate ModelControlMap in AutoForm.Blazor.GeneratedControls. Make sure that the AutoForm.Blazor.Analysis.BlazorGenerator has run.");
            }

            ModelControlMap = (IDictionary<Type, Type>)modelControlMapField.GetValue(null)!;

            var modelTemplateMapField = type?
                .GetField("ModelTemplateMap");

            if (modelTemplateMapField == null)
            {
                throw new Exception("Unable to locate ModelTemplateMap in AutoForm.Blazor.GeneratedControls. Make sure that the AutoForm.Blazor.Analysis.BlazorGenerator has run.");
            }

            ModelTemplateMap = (IDictionary<Type, Type>)modelTemplateMapField.GetValue(null)!;
        }

        public static IDictionary<Type, Type> ModelControlMap { get; }
        public static IDictionary<Type, Type> ModelTemplateMap { get; }
    }
}
