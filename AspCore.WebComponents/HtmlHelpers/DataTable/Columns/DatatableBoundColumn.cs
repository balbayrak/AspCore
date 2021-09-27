using System;
using System.Collections.Generic;
using System.Reflection;
using AspCore.WebComponents.HtmlHelpers.DataTable.Columns.Buttons;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Columns
{
    public class DatatableBoundColumn<TModel>
    {
        public bool columnIsPrimaryKey { get; set; }
        public string columnProperty { get; set; }
        public string column_Property_Exp { get; set; }
        public string orderByDirection { get; set; }
        public string searchable { get; set; }
        public Condition condition { get; set; }

    }
}
