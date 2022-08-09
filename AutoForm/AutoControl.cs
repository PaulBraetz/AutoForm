using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using System.Reflection;

namespace AutoForm
{
	public abstract class AutoControlBase:ComponentBase
	{
		static AutoControlBase()
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

		protected static IDictionary<Type, Type> ModelControlMap { get; private set; }
	}
	public sealed class AutoControl<TModel> : AutoControlBase
	{
		[Parameter]
		public TModel? Value { get; set; }
		[Parameter]
		public EventCallback<TModel?> ValueChanged { get; set; }

		private static readonly Type _controlType = ModelControlMap.TryGetValue(typeof(TModel), out Type? controlType) ?
			controlType :
			throw new Exception($"Unable to locate control for {nameof(TModel)} of {typeof(TModel).FullName}. Make sure that {typeof(TModel).FullName}is annotated with {nameof(AutoControlModelAttribute)}.");

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent(0, _controlType);
			builder.AddAttribute(1, "Value", RuntimeHelpers.TypeCheck(Value));
			builder.AddAttribute(2, "ValueChanged", RuntimeHelpers.TypeCheck(EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => Value = __value, Value))));
			builder.CloseComponent();
		}
	}
}
