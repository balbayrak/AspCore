using AspCore.Entities.Expression;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableBoundColumn : ITableColumn<ITableBoundColumn>
    {
        ITableBoundColumn IsPrimaryKey(bool value);

        ITableBoundColumn OrderBy(EnumSortingDirection direciton);

        ITableBoundColumn Searchable(Operation operation);
    }
}
