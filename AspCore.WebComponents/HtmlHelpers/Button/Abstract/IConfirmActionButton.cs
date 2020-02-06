using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;

namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface IConfirmActionButton : IActionButton<IConfirmActionButton>
    {
        IConfirmActionButton ConfirmOption(ConfirmOption confimoption);
    }
}
