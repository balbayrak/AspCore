using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.ModelBinder
{
    public class JQueryDataTablesModelBinderAttribute : ModelBinderAttribute
    {
        public JQueryDataTablesModelBinderAttribute() : base(typeof(JQueryDataTablesModelBinder))
        {

        }
    }
}
