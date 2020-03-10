using AspCore.WebComponents.HtmlHelpers.DataTable.Columns;
using AspCore.WebComponents.HtmlHelpers.DataTable.Rows;
using AspCore.WebComponents.HtmlHelpers.DataTable.Toolbar;
using AspCore.WebComponents.HtmlHelpers.General;
using AspCore.WebComponents.HtmlHelpers.General.Enums;
using System;
using System.Drawing;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableBuilder<TModel> : IFluentInterface where TModel : class
    {
        ITableBuilder<TModel> Columns(Action<ColumnBuilder<TModel>> columnBuilder);
        ITableBuilder<TModel> ToolBarActions(Action<ToolBarBuilder<TModel>> toolBarBuilder, TableExportSetting exportSetting);
        ITableBuilder<TModel> Portlet(string title, Color color, string iClass);
        ITableBuilder<TModel> PagingType(EnumPagingType pagingType);
        ITableBuilder<TModel> CssClass(string cssClass);
        ITableBuilder<TModel> Searching(bool searchable);
        ITableBuilder<TModel> StateSave(bool stateSave);
        ITableBuilder<TModel> RowCssConditions(Action<RowCssBuilder<TModel>> rowCssBuilder);

    }
}
