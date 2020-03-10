using AspCore.WebComponents.HtmlHelpers.General;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableRowCssBuilder<TModel> : IFluentInterface
           where TModel : class
    {
        ITableRowCssBuilder<TModel> AddRowCss<TProperty>(Expression<Func<TModel, TProperty>> expression, object value, string css);
    }
}
