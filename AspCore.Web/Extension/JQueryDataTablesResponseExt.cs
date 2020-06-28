using AspCore.Entities.DataTable;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.WebComponents.HtmlHelpers.DataTable.Storage;
using AspCore.WebComponents.HtmlHelpers.Extensions;
using System.Collections.Generic;

namespace AspCore.Web.Extension
{
    public static class JQueryDataTablesResponseExt
    {
        public static JQueryDataTablesResponse ToJQueryDataTablesResponse<TEntity>(this ServiceResult<List<TEntity>> result, JQueryDataTablesModel jQueryDataTablesModel) where TEntity : class, IEntity
        {
            var storageObject = jQueryDataTablesModel.columnInfos.DeSerialize<TEntity>();
            if (storageObject != null)
            {
                if (result.IsSucceeded && result.Result != null)
                {
                    using (var parser = new DatatableParser<TEntity>(result.Result, storageObject))
                    {
                        return parser.Parse(jQueryDataTablesModel, result.TotalResultCount, result.SearchResultCount);
                    }
                }
            }

            return null;
        }
    }
}
