using AspCore.WebComponents.HtmlHelpers.DataTable.Columns;
using AspCore.WebComponents.HtmlHelpers.DataTable.Toolbar;
using System;
using System.Drawing;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableBuilder<TModel> where TModel : class
    {
        TableBuilder<TModel> Columns(Action<ColumnBuilder<TModel>> columnBuilder);
        TableBuilder<TModel> ToolBarActions(Action<ToolBarBuilder<TModel>> toolBarBuilder, TableExportSetting exportSetting);
        TableBuilder<TModel> Portlet(string title, Color color, string iClass);
    }
}
