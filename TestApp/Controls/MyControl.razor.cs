using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TestApp.Models.NestedNamespace;

namespace TestApp.Controls
{
    [AutoControl(typeof(System.String))]
    public partial class MyControl
    {
        [Parameter]
        public String? Value { get; set; }
        [Parameter]
        public EventCallback<String?> ValueChanged {get;set;}

        private String InternalValue
        {
            get => Value;
            set
            {
                Value = value;
                ValueChanged.InvokeAsync(Value);
            }
        }
    }
}
