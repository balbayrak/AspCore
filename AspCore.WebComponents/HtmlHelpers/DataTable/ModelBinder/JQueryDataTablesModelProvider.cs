using AspCore.Entities.DataTable;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.ModelBinder
{
    public class JQueryDataTablesModelProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(JQueryDataTablesModel))
                return new JQueryDataTablesModelBinder();
            return null;
        }
    }
}
