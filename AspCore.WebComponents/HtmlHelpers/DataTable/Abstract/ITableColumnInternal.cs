﻿namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableColumnInternal
    {
        string tableid { get; set; }
        string columnTitle { get; set; }
        int width { get; set; }
        string HtmlColumn();
    }
}
