using AspCore.WebComponents.HtmlHelpers.DataTable.Columns.Buttons;
using System.Collections.Generic;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Columns
{
    public class DatatableActionColumn
    {
        public string ActionColumn { get; set; }

        public string ActionColumnHeader { get; set; }

        public Dictionary<int, Condition> conditions { get; set; }
    }
}