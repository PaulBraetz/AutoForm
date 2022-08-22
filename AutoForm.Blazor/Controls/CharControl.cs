using AutoForm.Blazor.Attributes;
using AutoForm.Blazor.Controls.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoForm.Blazor.Controls
{
	public class CharControl:InputControlBase<Char>
    {
        public CharControl() : base("text")
        {

        }
        private static readonly AttributeCollection _additionalAttributes = new AttributeCollection("maxlength", "1")
            ;
        protected override AttributeCollection GetAdditionalAttributes()
        {
            return AttributeCollection.Union(_additionalAttributes, base.GetAdditionalAttributes());
        }
    }
}
