using System.Reflection;

namespace AutoForm
{
	public static class Controls
	{
		static Controls()
		{
			Type? type = Assembly.GetEntryAssembly()?
				.GetType("AutoForm.Generate.Controls");
			if (type == null)
			{
				throw new Exception("Unable to locate AutoForm.Generate.Controls. Make sure that the AutoForm.Generate.Generator has run.");
			}

			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

			FieldInfo? field = type?
				.GetField("ModelControlMap");

			if (field == null)
			{
				throw new Exception("Unable to locate ModelControlMap in AutoForm.Generate.Controls. Make sure that the AutoForm.Generate.Generator has run.");
			}

			ModelControlMap = (IDictionary<Type, Type>)field.GetValue(null)!;
		}

		public static IDictionary<Type, Type> ModelControlMap { get; private set; }
	}
}
