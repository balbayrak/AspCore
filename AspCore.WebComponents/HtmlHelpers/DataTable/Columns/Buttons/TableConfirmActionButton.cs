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
    public class TableConfirmActionButton<TModel> : ConfirmActionButton, ITableActionButton<IConfirmActionButton, TModel>
            where TModel : class
    {
        public TableConfirmActionButton() : base(string.Empty)
        {

        }

        public IConfirmActionButton Visible(bool visible)
        {
            return _instance;
        }

        public IConfirmActionButton Hidden<TProperty>(Expression<Func<TModel, TProperty>> expression, object value)
        {
            this.condition = expression.ToCondition(value);
            return _instance;
        }
    }
}
