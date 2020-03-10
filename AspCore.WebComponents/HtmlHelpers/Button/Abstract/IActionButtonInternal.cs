using AspCore.WebComponents.HtmlHelpers.Block;
using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using AspCore.WebComponents.HtmlHelpers.DataTable.Columns.Buttons;

namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface IActionButtonInternal
    {
        string id { get; set; }
        ActionInfo action { get; set; }
        string text { get; set; }
        string cssClass { get; set; }
        string iclass { get; set; }
        BlockInfo block { get; set; }
        string CreateLink();
        Condition condition { get; set; }

    }
}
