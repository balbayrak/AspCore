using System.Collections.Generic;
using AspCore.WebComponents.HtmlHelpers.DataTable.Columns.Buttons;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableBoundColumnInternal<TModel> : ITableColumnInternal where TModel : class
    {
        bool columnIsPrimaryKey { get; set; }
        string columnProperty { get; set; }
        string columnPropertyExp { get; set; }
        string orderByDirection { get; set; }
        string searchable { get; set; }
        
        Condition condition { get; set; }

    }
}
