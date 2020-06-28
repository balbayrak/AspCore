using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using AspCore.WebComponents.HtmlHelpers.DataTable.Abstract;
using AspCore.WebComponents.HtmlHelpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Columns.Buttons
{
    public class TableDefaultActionButton<TModel> : DefaultActionButton, ITableActionButton<IDefaultActionButton, TModel>
        where TModel : class
    {
        public TableDefaultActionButton() : base(string.Empty)
        {

        }

        public IDefaultActionButton Visible(bool visible)
        {
            return _instance;
        }

        public IDefaultActionButton Hidden<TProperty>(Expression<Func<TModel, TProperty>> expression, object value)
        {
            this.condition = expression.ToCondition(value);
            return _instance;
        }
    }
}
