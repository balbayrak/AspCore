using System;
using System.Linq.Expressions;
using AspCore.Entities.Expression;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableBoundColumn<TModel, TProperty> : ITableColumn<ITableBoundColumn<TModel, TProperty>>
    {
        ITableBoundColumn<TModel, TProperty> IsPrimaryKey(bool value);

        ITableBoundColumn<TModel, TProperty> OrderBy(EnumSortingDirection direciton);

        ITableBoundColumn<TModel, TProperty> Searchable(Operation operation);
        ITableBoundColumn<TModel, TProperty> HiddenFor(Expression<Func<TModel, bool>> expression);

    }
}
