using System;
using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.DataTable;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.WebComponents.HtmlHelpers.DataTable.Storage;
using AspCore.WebComponents.HtmlHelpers.Extensions;
using System.Collections.Generic;
using System.Linq;
using AspCore.Dependency.Concrete;
using AspCore.DataSearch.Abstract;

namespace AspCore.Web.Concrete
{
    public abstract class BaseDatatableSearchableEntityBffLayer<TViewModel, TSearchableEntity, TSearchClient> : BaseSearchableEntityBffLayer<TViewModel, TSearchableEntity, TSearchClient>
        where TViewModel : BaseViewModel<TSearchableEntity>, new()
        where TSearchableEntity : class, ISearchableEntity, new()
        where TSearchClient : class, IDataSearchClient<TSearchableEntity>
    {
        public virtual JQueryDataTablesResponse GetAll(JQueryDataTablesModel jQueryDataTablesModel)
        {

            try
            {
                var storageObject = jQueryDataTablesModel.columnInfos.DeSerialize<TSearchableEntity>();
                if (storageObject != null)
                {
                    var entityFilter = jQueryDataTablesModel.ToEntityFilter<TSearchableEntity>(storageObject.GetSearchableColumnString());
                    int startIndex = entityFilter.page.Value * entityFilter.pageSize.Value;
                    ServiceResult<List<TViewModel>> result = FindBy(true, startIndex, entityFilter.pageSize.Value);
                    if (result.IsSucceeded && result.Result != null)
                    {
                        using (var parser = new DatatableParser<TSearchableEntity>(result.Result.Select(t => t.dataEntity).ToList(), storageObject))
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
