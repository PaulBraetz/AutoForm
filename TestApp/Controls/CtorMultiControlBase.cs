namespace TestApp.Controls
{
    public class CtorMultiControlBase<TModel> : MultiControlBase<TModel>
        where TModel : new()
    {
        protected override Func<TModel> ModelFactory => () => new TModel();
    }
}
