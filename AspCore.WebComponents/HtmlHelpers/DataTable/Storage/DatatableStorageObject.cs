using AspCore.WebComponents.HtmlHelpers.DataTable.Columns;
using System.Collections.Generic;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Storage
{
    public class DatatableStorageObject<TEntity>
    where TEntity : class
    {
        #region Public Properties

        public List<DatatableBoundColumn<TEntity>> DatatableProperties { get; set; }

        public List<DatatableActionColumn> DatatableActions { get; set; }

        #endregion
    }
}