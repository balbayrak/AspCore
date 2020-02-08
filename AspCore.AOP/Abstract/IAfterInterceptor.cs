namespace AspCore.AOP.Abstract
{
    public interface IAfterInterceptor : IInterceptor
    {
        void OnAfter();
    }
}
