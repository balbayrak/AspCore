using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using System.Collections.Generic;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableActionColumnInternal : ITableColumnInternal
    {
        List<IActionButtonInternal> actions { get; set; }
        void AddAction(IActionButtonInternal button);
        int actionColumnIndex { get; set; }
        string columnActionsModalHtml { get; }
        string columnActionsHtml { get; }
        string columnDataProperty { get; }
    }
}
