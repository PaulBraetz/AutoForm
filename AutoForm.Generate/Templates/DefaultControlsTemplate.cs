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
				{typeof(String).FullName, INPUT_TEXT_IDENTIFIER }
			});
			private readonly static IDictionary<String, String> AvailableDefaultControlTemplates = new ReadOnlyDictionary<String, String>(new Dictionary<String, String>()
			{
				{typeof(String).FullName, INPUT_TEXT }
			});

			private const String INPUT_TEXT_IDENTIFIER = "InputText";
			private const String INPUT_TEXT =
@"		///<summary>
		///Default control for System.String
		///</summary>
		private sealed class " + INPUT_TEXT_IDENTIFIER + @" : __ControlBase<global::System.String>
		{
			#pragma warning disable 1998
			protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
			{
				__builder.OpenElement(0, ""input"");
				__builder.AddAttribute(1, ""value"", global::Microsoft.AspNetCore.Components.BindConverter.FormatValue(__Value));
				__builder.AddAttribute(2, ""oninput"", global::Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => __Value = __value, __Value));
				if(Attributes != null)
				{
					__builder.AddMultipleAttributes(3, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<string, object>>>(Attributes));
				}
				__builder.SetUpdatesAttributeName(""value"");
				__builder.CloseElement();
			}
			#pragma warning restore 1998

			private global::System.String __Value
			{
				get => Value;
				set
				{
					Value = value;
					ValueChanged.InvokeAsync(Value);
				}
			}
		}";

			private readonly IEnumerable<String> _requiredDefaultControls;

			public DefaultControlsTemplate WithRequiredDefaultControls(IEnumerable<String> requiredDefaultControls)
			{
				return new DefaultControlsTemplate(requiredDefaultControls);
			}

			public String Build()
			{
				IEnumerable<String> controls = _requiredDefaultControls
					.Select(c => AvailableDefaultControlTemplates[c]);
				return String.Join("\n\n", controls);
			}
		}
	}
}
