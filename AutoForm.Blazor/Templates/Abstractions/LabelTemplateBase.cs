using Microsoft.AspNetCore.Components.Rendering;

namespace AutoForm.Blazor.Templates.Abstractions
{
	public abstract class LabelTemplateBase<TModel> : TemplateBase<TModel>
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "label");
			if (Attributes != null)
			{
				var id = Attributes.FirstOrDefault(kvp => kvp.Key == "id").Value;
				if (id != null)
				{
					builder.AddAttribute(1, "for", id);
				}

				var label = Attributes.FirstOrDefault(kvp => kvp.Key == "label").Value;
				if (label != null)
				{
					builder.AddContent(2, label);
				}
			}

			builder.CloseElement();
		}
	}
}
