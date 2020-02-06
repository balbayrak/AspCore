using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using System;

namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface IGrupActionButton : IActionButton<IGrupActionButton>
    {
        IGrupActionButton SubActions(Action<SubActionBuilder> actionBuilder);
    }
}
