using Microsoft.AspNetCore.Components;

namespace TestApp.Controls
{
    public abstract partial class MultiControlBase<TModel> : ComponentBase
    {
        [Parameter]
        public ICollection<TModel> Value { get; set; }
        [Parameter]
        public EventCallback<ICollection<TModel>> ValueChanged { get; set; }
        [Parameter]
        public IDictionary<String, Object> Attributes { get; set; }

        protected abstract Func<TModel>? ModelFactory { get; }

#nullable disable
        private LinkedList<TModel> _values;
#nullable restore

        protected override void OnParametersSet()
        {
            _values = new LinkedList<TModel>(Value);
            base.OnParametersSet();
        }

        protected void Add()
        {
            _values.AddLast(new LinkedListNode<TModel>(ModelFactory!.Invoke()));
            Value.Add(_values.Last!.Value);
            ValueChanged.InvokeAsync(Value);
        }
        protected void Remove(TModel model)
        {
            Value.Remove(model);
            _values.Remove(model);
            ValueChanged.InvokeAsync(Value);
        }
        protected void SubControlChanged()
        {
            ValueChanged.InvokeAsync(Value);
        }
    }
}
