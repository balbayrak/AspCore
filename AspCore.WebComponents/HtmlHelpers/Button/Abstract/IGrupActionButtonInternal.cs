namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface IGrupActionButtonInternal : IActionButtonInternal
    {
        void AddAction(IActionButtonInternal button);
    }
}
