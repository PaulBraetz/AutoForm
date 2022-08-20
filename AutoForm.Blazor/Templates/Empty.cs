using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoForm.Blazor.Templates
{
    public sealed class Empty : ComponentBase
	{
		[Parameter]
		public Object? Value { get; set; }
		[Parameter]
		public IEnumerable<KeyValuePair<String, Object>>? Attributes { get; set; }
		[Parameter]
		public RenderFragment? ChildContent { get; set; }
		protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.AddContent(0, ChildContent);
        }
    }
}
