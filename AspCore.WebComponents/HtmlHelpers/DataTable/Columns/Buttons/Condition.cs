using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Columns.Buttons
{
    public class Condition
    {
        public string property { get; set; }

        public bool IsEqual { get; set; }
        public object value { get; set; }
    }
}
