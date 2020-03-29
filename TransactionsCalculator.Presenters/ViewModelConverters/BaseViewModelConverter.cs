namespace TransactionsCalculator.Presenters.ViewModelConverters
{
    public abstract class BaseViewModelConverter<TSource, TDestination>
    {
       

        public abstract TDestination Convert(TSource source);
    }
}
