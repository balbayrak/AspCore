namespace AspCore.AOP.Abstract
{
    public interface IBeforeInterceptor : IInterceptor
    {
        void OnBefore();
    }
}
