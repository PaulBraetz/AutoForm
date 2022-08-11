using System;

namespace AutoForm.Generate
{
	internal static partial class Source
	{
		private readonly struct DefaultNumberControlTemplate
		{
			private DefaultNumberControlTemplate(ControlTypeIdentifierTemplate controlTypeIdentifierTemplate, String modelType)
			{
				_controlTypeIdentifierTemplate = controlTypeIdentifierTemplate;
				_modelType = modelType;
			}

			private const String TEMPLATE =
@"		///<summary>
		///Default control for System." + MODEL_TYPE + @"
		///</summary>
		private sealed class " + CONTROL_TYPE_IDENTIFIER + @" : __ControlBase<global::" + MODEL_TYPE + @">
		{
			#pragma warning disable 1998
			protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
			{
				__builder.OpenElement(0, ""input"");
				__builder.AddAttribute(1, ""type"", ""number"");
				__builder.AddAttribute(2, ""value"", global::Microsoft.AspNetCore.Components.BindConverter.FormatValue(__Value));
				__builder.AddAttribute(3, ""oninput"", global::Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => __Value = __value, __Value));
				if(Attributes != null)
				{
					__builder.AddMultipleAttributes(4, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<global::System.String, global::System.Object>>>(Attributes));
				}
				__builder.SetUpdatesAttributeName(""value"");
				__builder.CloseElement();
			}

			private global::" + MODEL_TYPE + @" __Value
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

			private readonly ControlTypeIdentifierTemplate _controlTypeIdentifierTemplate;
			private readonly String _modelType;

			public DefaultNumberControlTemplate WithControlTypeIdentifierTemplate(ControlTypeIdentifierTemplate controlTypeIdentifierTemplate)
			{
				return new DefaultNumberControlTemplate(controlTypeIdentifierTemplate, _modelType);
			}
			public DefaultNumberControlTemplate WithModelType(String modelType)
			{
				return new DefaultNumberControlTemplate(_controlTypeIdentifierTemplate, modelType);
			}

			public String Build()
			{
				String controlTypeIdentifier = _controlTypeIdentifierTemplate.Build();

				return TEMPLATE
					.Replace(MODEL_TYPE, _modelType)
					.Replace(CONTROL_TYPE_IDENTIFIER, controlTypeIdentifier);
			}

            public override String ToString()
            {
				return Build();
            }
        }
	}
}
