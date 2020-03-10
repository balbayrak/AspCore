using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.General.Enums;
using System;
using System.Linq.Expressions;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableActionButton<T, TModel>
          where T : IActionButton<T>
          where TModel : class
    {
        T Visible(bool visible);

        T Hidden<TProperty>(Expression<Func<TModel, TProperty>> expression, object value);
    }
}
