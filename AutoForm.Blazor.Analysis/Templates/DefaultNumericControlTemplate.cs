using AutoForm.Analysis.Models;
using System;

namespace AutoForm.Generate.Blazor.Templates
{
	internal sealed partial class SourceFactory
	{
		private readonly struct DefaultNumericControlTemplate
		{
			private DefaultNumericControlTemplate(TypeIdentifier controlType, TypeIdentifier modelType)
			{
				_controlType = controlType;
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
					__builder.AddMultipleAttributes(4, global::Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<global::System.Collections.Generic.IDictionary<global::System.String, global::System.Object>>(Attributes));
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

			private readonly TypeIdentifier _controlType;
			private readonly TypeIdentifier _modelType;

			public DefaultNumericControlTemplate WithControlType(TypeIdentifier controlType)
			{
				return new DefaultNumericControlTemplate(controlType, _modelType);
			}
			public DefaultNumericControlTemplate WithModelType(TypeIdentifier modelType)
			{
				return new DefaultNumericControlTemplate(_controlType, modelType);
			}

			public String Build()
			{
				return TEMPLATE
					.Replace(MODEL_TYPE, _modelType.ToEscapedString())
					.Replace(CONTROL_TYPE_IDENTIFIER, _controlType.ToEscapedString());
			}

			public override String ToString()
			{
				return Build();
			}
		}
	}
}