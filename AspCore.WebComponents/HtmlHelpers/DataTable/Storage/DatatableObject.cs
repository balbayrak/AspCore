using System.Collections.Generic;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Storage
{
    public class DatatableObject<TEntity>
            where TEntity : class
    {
        public string controllerName { get; set; }

        public Dictionary<string,string> dataTablesDictionary { get; set; }

    }
}
