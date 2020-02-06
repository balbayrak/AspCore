using AspCore.Entities.DataTable;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AspCore.WebComponents.HtmlHelpers.Extensions
{
    public static class JQueryDataTablesModelExt
    {
        public static EntityFilter<TEntity> ToEntityFilter<TEntity>(this JQueryDataTablesModel model, string searchableColumns = null) where TEntity : class, IEntity, new()
        {
            List<SortingType<TEntity>> sortings = null;
            ReadOnlyCollection<SortingColumn> sorterColumns = model.GetSortedColumns();
            if (sorterColumns != null && sorterColumns.Count > 0)
            {
                sortings = new List<SortingType<TEntity>>();
                foreach (var item in sorterColumns)
                {
                    sortings.Add(new SortingType<TEntity>(item.propertyName, item.sortDirection));
                }
            }
            SearchType searchType = null;

            if (!string.IsNullOrEmpty(searchableColumns?.Trim()))
            {
                searchType = new SearchType
                {
                    searchableColumns = searchableColumns,
                    searchValue = model.sSearch
                };
            }

            return new EntityFilter<TEntity>
            {
                page = model.iDisplayStart != 0 ? model.iDisplayStart / model.iDisplayLength : 0,
                pageSize = model.iDisplayLength,
                sorters = sortings,
                search = searchType
            };
        }
    }
}
