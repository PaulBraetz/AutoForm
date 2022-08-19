using AutoForm.Analysis.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoForm.Generate.Blazor.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct DefaultControlsTemplate
		{
			private DefaultControlsTemplate(TypeIdentifier[] requiredDefaultControls)
			{
				_requiredDefaultControls = requiredDefaultControls ?? Array.Empty<TypeIdentifier>();
			}

			public readonly static IDictionary<TypeIdentifier, TypeIdentifier> DefaultModelControlPairs = new ReadOnlyDictionary<TypeIdentifier, TypeIdentifier>(new Dictionary<TypeIdentifier, TypeIdentifier>()
			{
				{TypeIdentifier.Create<String>(), GetDefaultControlIdentifier<String>() },
				{TypeIdentifier.Create<Boolean>(), GetDefaultControlIdentifier<Boolean>() },
				{TypeIdentifier.Create<Byte>(), GetDefaultControlIdentifier<Byte>() },
				{TypeIdentifier.Create<SByte>(), GetDefaultControlIdentifier<SByte>() },
				{TypeIdentifier.Create<Int16>(), GetDefaultControlIdentifier<Int16>() },
				{TypeIdentifier.Create<UInt16>(), GetDefaultControlIdentifier<UInt16>() },
				{TypeIdentifier.Create<Int32>(), GetDefaultControlIdentifier<Int32>() },
				{TypeIdentifier.Create<UInt32>(), GetDefaultControlIdentifier<UInt32>() },
				{TypeIdentifier.Create<Int64>(), GetDefaultControlIdentifier<Int64>() },
				{TypeIdentifier.Create<UInt64>(), GetDefaultControlIdentifier<UInt64>() },
				{TypeIdentifier.Create<Single>(), GetDefaultControlIdentifier<Single>() },
				{TypeIdentifier.Create<Double>(), GetDefaultControlIdentifier<Double>() },
				{TypeIdentifier.Create<Decimal>(), GetDefaultControlIdentifier<Decimal>() }
			});
			private readonly static IDictionary<TypeIdentifier, String> DefaultModelTemplatePairs = new ReadOnlyDictionary<TypeIdentifier, String>(new Dictionary<TypeIdentifier, String>()
			{
				{TypeIdentifier.Create<String>(), STRING_CONTROL },
				{TypeIdentifier.Create<Boolean>(), BOOLEAN_CONTROL},
				{TypeIdentifier.Create<Byte>(), GetNumberControl<Byte>() },
				{TypeIdentifier.Create<SByte>(), GetNumberControl<SByte>() },
				{TypeIdentifier.Create<Int16>(), GetNumberControl<Int16>() },
				{TypeIdentifier.Create<UInt16>(), GetNumberControl<UInt16>() },
				{TypeIdentifier.Create<Int32>(), GetNumberControl<Int32>() },
				{TypeIdentifier.Create<UInt32>(), GetNumberControl<UInt32>() },
				{TypeIdentifier.Create<Int64>(), GetNumberControl<Int64>() },
				{TypeIdentifier.Create<UInt64>(), GetNumberControl<UInt64>() },
				{TypeIdentifier.Create<Single>(), GetNumberControl<Single>() },
				{TypeIdentifier.Create<Double>(), GetNumberControl<Double>() },
				{TypeIdentifier.Create<Decimal>(), GetNumberControl<Decimal>() }
			});

			private static TypeIdentifier GetDefaultControlIdentifier<T>()
			{
				return TypeIdentifier.CreateGeneratedControl(TypeIdentifier.Create<T>());
			}
			private static String GetNumberControl<T>()
			{
				var modelType = TypeIdentifier.Create<T>();

				var controlType = GetDefaultControlIdentifier<T>();

				return new DefaultNumericControlTemplate()
					.WithModelType(modelType)
					.WithControlType(controlType)
					.Build();
			}

			private const String STRING_CONTROL_IDENTIFIER = "{" + nameof(STRING_CONTROL_IDENTIFIER) + "}";
			private const String STRING_CONTROL =
	@"		///<summary>
		///Default control for System.String
		///</summary>
		private sealed class " + STRING_CONTROL_IDENTIFIER + @" : __ControlBase<global::System.String>
		{
			#pragma warning disable 1998
			protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
			{
				__builder.OpenElement(0, (__Value?.Length ?? 0) > 128 ? ""textarea"" : ""input"");
				__builder.AddAttribute(1, ""value"", global::Microsoft.AspNetCore.Components.BindConverter.FormatValue(__Value));
				__builder.AddAttribute(2, ""oninput"", global::Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => __Value = __value, __Value));
				if(Attributes != null)
				{
					__builder.AddMultipleAttributes(3, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IDictionary<global::System.String, global::System.Object>>(Attributes));
				}
				__builder.SetUpdatesAttributeName(""value"");
				__builder.CloseElement();
			}

			private global::System.String __Value
			{
				get => Value;
				set
				{
					Value = value;
					ValueChanged.InvokeAsync(Value);
				}
			}
			#pragma warning restore 1998
		}";

			private const String BOOLEAN_CONTROL_IDENTIFIER = "{" + nameof(BOOLEAN_CONTROL_IDENTIFIER) + "}";
			private const String BOOLEAN_CONTROL =
	@"		///<summary>
		///Default control for System.Boolean
		///</summary>
		private sealed class " + BOOLEAN_CONTROL_IDENTIFIER + @" : __ControlBase<global::System.Boolean>
		{
			#pragma warning disable 1998
			protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
			{
				__builder.OpenElement(0, ""input"");
				__builder.AddAttribute(1, ""type"", ""checkbox"");
				__builder.AddAttribute(2, ""checked"", global::Microsoft.AspNetCore.Components.BindConverter.FormatValue(Value));				
				__builder.AddAttribute(3, ""oninput"", global::Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => __Value = __value, __Value));
				if(Attributes != null)
				{
					__builder.AddMultipleAttributes(4, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IDictionary<global::System.String, global::System.Object>>(Attributes));
				}
				__builder.SetUpdatesAttributeName(""checked"");
				__builder.CloseElement();
			}

			private global::System.Boolean __Value
			{
				get => Value;
				set
				{
					Value = value;
					ValueChanged.InvokeAsync(Value);
				}
			}
			#pragma warning restore 1998
		}
";

			private readonly TypeIdentifier[] _requiredDefaultControls;

			public DefaultControlsTemplate WithRequiredDefaultControls(IEnumerable<TypeIdentifier> requiredDefaultControls)
			{
				return new DefaultControlsTemplate(requiredDefaultControls.ToArray());
			}

			public String Build()
			{
				IEnumerable<String> controlTemplates = _requiredDefaultControls
					.Select(c => DefaultModelTemplatePairs[c]);

				return String.Join("\n\n", controlTemplates)
					.Replace(STRING_CONTROL_IDENTIFIER, DefaultModelControlPairs[TypeIdentifier.Create<String>()].ToEscapedString())
					.Replace(BOOLEAN_CONTROL_IDENTIFIER, DefaultModelControlPairs[TypeIdentifier.Create<Boolean>()].ToEscapedString());
			}

			public override String ToString()
			{
				return Build();
			}
		}
	}
}