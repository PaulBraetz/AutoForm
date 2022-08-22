namespace AutoForm.Blazor.Controls.Abstractions
{
	public abstract class OptimizedControlBase<TModel> : ControlBase<TModel>
    {
        private static IEqualityComparer<TModel> _equalityComparer = EqualityComparer<TModel>.Default;
        private TModel _previousValue;
        private Boolean _shouldRender;

        protected override void OnParametersSet()
        {
            _shouldRender = !_equalityComparer.Equals(Value, _previousValue);
            _previousValue = Value;
            base.OnParametersSet();
        }

        protected override Boolean ShouldRender()
        {
            return _shouldRender;
        }
    }
}
