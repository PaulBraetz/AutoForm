namespace AutoForm.Blazor.Controls.Abstractions
{
    public abstract class NumberControlBase<TModel> : InputControlBase<TModel>
    {
        protected NumberControlBase() : base("number") { }
    }
}
