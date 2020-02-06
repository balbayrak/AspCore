namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableCheckColumnInternal : ITableColumnInternal
    {
        int actionColumnIndex { get; set; }
        string columnDataProperty { get; }
        string checkActionHtml { get; }
        bool checkAllEnabled { get; set; }
    }
}
