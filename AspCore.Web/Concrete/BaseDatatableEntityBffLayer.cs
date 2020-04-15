using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.DataTable;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.WebComponents.HtmlHelpers.DataTable.Storage;
using AspCore.WebComponents.HtmlHelpers.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.Web.Concrete
{
    public abstract class BaseDatatableEntityBffLayer<TViewModel, TEntity> : BaseEntityBffLayer<TViewModel, TEntity>
        where TViewModel : BaseViewModel<TEntity>, new()
        where TEntity : class, IEntity, new()
    {
        public virtual JQueryDataTablesResponse GetAll(JQueryDataTablesModel jQueryDataTablesModel)
        {
           
            try
            {
                var storageObject = jQueryDataTablesModel.columnInfos.DeSerialize<TEntity>();
                if (storageObject != null)
                {
                    ServiceResult<List<TViewModel>> result = GetAllAsync(jQueryDataTablesModel.ToEntityFilter<TEntity>(storageObject.GetSearchableColumnString())).Result;
                    if (result.IsSucceeded && result.Result != null)
                    {
                        using (var parser = new DatatableParser<TEntity>(result.Result.Select(t => t.dataEntity).ToList(), storageObject))
                        {
                            return parser.Parse(jQueryDataTablesModel, result.TotalResultCount, result.SearchResultCount);
                        }

                    }
                }
            }
            catch
            {
                return null;
                // ignored
            }

            return null;
        }
    }
}
