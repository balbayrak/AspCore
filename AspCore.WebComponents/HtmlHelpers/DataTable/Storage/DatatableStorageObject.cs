using AspCore.WebComponents.HtmlHelpers.DataTable.Columns;
using AspCore.WebComponents.HtmlHelpers.DataTable.Rows;
using System.Collections.Generic;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Storage
{
    public class DatatableStorageObject<TEntity>
 
    {
        #region Public Properties

        public List<DatatableBoundColumn<TEntity>> DatatableProperties { get; set; }

        public List<DatatableActionColumn> DatatableActions { get; set; }

        public List<RowCondition> RowConditions { get; set; }

        #endregion
    }
}