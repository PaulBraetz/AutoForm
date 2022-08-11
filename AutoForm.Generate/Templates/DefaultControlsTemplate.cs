using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct DefaultControlsTemplate
		{
			private DefaultControlsTemplate(IEnumerable<String> requiredDefaultControls)
			{
				_requiredDefaultControls = requiredDefaultControls;
			}

			public readonly static IDictionary<String, String> AvailableDefaultControls = new ReadOnlyDictionary<String, String>(new Dictionary<String, String>()
			{
				{typeof(String).FullName, GetNumberControlIdentifier(typeof(String)) },
				{typeof(Boolean).FullName, GetNumberControlIdentifier(typeof(Boolean)) },
				{typeof(Byte).FullName, GetNumberControlIdentifier(typeof(Byte)) },
				{typeof(SByte).FullName, GetNumberControlIdentifier(typeof(SByte)) },
				{typeof(Int16).FullName, GetNumberControlIdentifier(typeof(Int16)) },
				{typeof(UInt16).FullName, GetNumberControlIdentifier(typeof(UInt16)) },
				{typeof(Int32).FullName, GetNumberControlIdentifier(typeof(Int32)) },
				{typeof(UInt32).FullName, GetNumberControlIdentifier(typeof(UInt32)) },
				{typeof(Int64).FullName, GetNumberControlIdentifier(typeof(Int64)) },
				{typeof(UInt64).FullName, GetNumberControlIdentifier(typeof(UInt64)) },
				{typeof(Single).FullName, GetNumberControlIdentifier(typeof(Single)) },
				{typeof(Double).FullName, GetNumberControlIdentifier(typeof(Double)) },
				{typeof(Decimal).FullName, GetNumberControlIdentifier(typeof(Decimal)) }
			});
			private readonly static IDictionary<String, String> AvailableDefaultControlTemplates = new ReadOnlyDictionary<String, String>(new Dictionary<String, String>()
			{
				{typeof(String).FullName, STRING_CONTROL },
				{typeof(Boolean).FullName, BOOLEAN_CONTROL},
				{typeof(Byte).FullName, GetNumberControl(typeof(Byte)) },
				{typeof(SByte).FullName, GetNumberControl(typeof(SByte)) },
				{typeof(Int16).FullName, GetNumberControl(typeof(Int16)) },
				{typeof(UInt16).FullName, GetNumberControl(typeof(UInt16)) },
				{typeof(Int32).FullName, GetNumberControl(typeof(Int32)) },
				{typeof(UInt32).FullName, GetNumberControl(typeof(UInt32)) },
				{typeof(Int64).FullName, GetNumberControl(typeof(Int64)) },
				{typeof(UInt64).FullName, GetNumberControl(typeof(UInt64)) },
				{typeof(Single).FullName, GetNumberControl(typeof(Single)) },
				{typeof(Double).FullName, GetNumberControl(typeof(Double)) },
				{typeof(Decimal).FullName, GetNumberControl(typeof(Decimal)) }
			});

			private static String GetNumberControlIdentifier(Type modelType)
			{
				return new ControlTypeIdentifierTemplate()
					.WithModelType(modelType.FullName)
					.Build();
			}
			private static String GetNumberControl(Type modelType)
			{
				var controlIdentifierTemplate = new ControlTypeIdentifierTemplate()
														.WithModelType(modelType.FullName);

				return new DefaultNumberControlTemplate()
					.WithModelType(modelType.FullName)
					.WithControlTypeIdentifierTemplate(controlIdentifierTemplate)
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
				__builder.OpenElement(0, ""input"");
				__builder.AddAttribute(1, ""value"", global::Microsoft.AspNetCore.Components.BindConverter.FormatValue(__Value));
				__builder.AddAttribute(2, ""oninput"", global::Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => __Value = __value, __Value));
				if(Attributes != null)
				{
					__builder.AddMultipleAttributes(3, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<global::System.String, global::System.Object>>>(Attributes));
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
					__builder.AddMultipleAttributes(4, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<global::System.String, global::System.Object>>>(Attributes));
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

			private readonly IEnumerable<String> _requiredDefaultControls;

			public DefaultControlsTemplate WithRequiredDefaultControls(IEnumerable<String> requiredDefaultControls)
			{
				return new DefaultControlsTemplate(requiredDefaultControls);
			}

			public String Build()
			{
				IEnumerable<String> controls = _requiredDefaultControls
					.Select(c => AvailableDefaultControlTemplates[c]);

				return String.Join("\n\n", controls)
					.Replace(STRING_CONTROL_IDENTIFIER, AvailableDefaultControls[typeof(String).FullName])
					.Replace(BOOLEAN_CONTROL_IDENTIFIER, AvailableDefaultControls[typeof(Boolean).FullName]);
			}

            public override String ToString()
            {
				return Build();
            }
        }
	}
}
