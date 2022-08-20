using Microsoft.AspNetCore.Components;

namespace TestApp.Controls
{
    public class MultiControl<TModel> : MultiControlBase<TModel>
    {
        [Parameter]
        public Func<TModel>? Factory { get; set; }

        protected override Func<TModel>? ModelFactory => Factory;
    }
}
