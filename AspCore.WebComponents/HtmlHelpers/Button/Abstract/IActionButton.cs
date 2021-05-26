using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using AspCore.WebComponents.HtmlHelpers.General;

namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface IActionButton<T> : IFluentInterface where T : IActionButton<T>
    {
        T ActionInfo(ActionInfo action);
        T Text(string text);
        T CssClass(string cssClass);
        T IClass(string iclass);
        T AddHref(string href,string prop);
        T BlockUI(string blocktarget = null);
    }

}
