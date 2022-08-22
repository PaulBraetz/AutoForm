using AutoForm.Blazor.Attributes;
using Fort;
using System.Collections.ObjectModel;

namespace AutoForm.Blazor.Controls.Abstractions
{
    public abstract class InputControlBase<TModel> : PrimitiveControlBase<TModel>
    {
        protected InputControlBase(String type, String updatesAttributeName = "value") : base("input", updatesAttributeName)
        {
            type.ThrowIfDefault(nameof(type));

            _additionalAttributes = new AttributeCollection("type", type);
        }

        private readonly AttributeCollection _additionalAttributes;

        protected override AttributeCollection GetAdditionalAttributes()
        {
            return AttributeCollection.Union(_additionalAttributes, base.GetAdditionalAttributes());
        }
    }
}
