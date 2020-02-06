using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using System;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableActionColumn : ITableColumn<ITableActionColumn>
    {
        ITableActionColumn Actions(Action<ActionBuilder> actionBuilder);
    }
}
