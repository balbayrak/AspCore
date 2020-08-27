using AspCore.Entities.DataTable;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AspCore.WebComponents.HtmlHelpers.Extensions
{
    public static class JQueryDataTablesModelExt
    {
        public static EntityFilter ToEntityFilter(this JQueryDataTablesModel model, string searchableColumns = null)
        {
            List<SortingType> sortings = null;
            ReadOnlyCollection<SortingColumn> sorterColumns = model.GetSortedColumns();
            if (sorterColumns != null && sorterColumns.Count > 0)
            {
                sortings = new List<SortingType>();
                foreach (var item in sorterColumns)
                {
                    sortings.Add(new SortingType(item.propertyName, item.sortDirection));
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

            return new EntityFilter
            {
                page = model.iDisplayStart != 0 ? model.iDisplayStart / model.iDisplayLength : 0,
                pageSize = model.iDisplayLength,
                sorters = sortings,
                search = searchType
            };
        }

        public static T ToEntityFilter<T>(this JQueryDataTablesModel model, string searchableColumns = null)
            where T : EntityFilter, new()
        {
            List<SortingType> sortings = null;
            ReadOnlyCollection<SortingColumn> sorterColumns = model.GetSortedColumns();
            if (sorterColumns != null && sorterColumns.Count > 0)
            {
                sortings = new List<SortingType>();
                foreach (var item in sorterColumns)
                {
                    sortings.Add(new SortingType(item.propertyName, item.sortDirection));
                }
            }
            SearchType searchType = new SearchType
            {
                searchableColumns = searchableColumns?.Trim(),
                searchValue = model.sSearch
            };


            return new T
            {
                page = model.iDisplayStart != 0 ? model.iDisplayStart / model.iDisplayLength : 0,
                pageSize = model.iDisplayLength,
                sorters = sortings,
                search = searchType
            };
        }
    }
}
