using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using System;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableActionColumn<TModel> : ITableColumn<ITableActionColumn<TModel>>
        where TModel : class
    {
        ITableActionColumn<TModel> Actions(Action<ActionBuilder<TModel>> actionBuilder);
    }
}
