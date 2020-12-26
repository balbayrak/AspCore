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
    public class TableModalActionButton<TModel> : ModalActionButton, ITableActionButton<IModalActionButton, TModel>
        where TModel : class
    {
        public TableModalActionButton() : base(string.Empty)
        {

        }

        public IModalActionButton Visible(bool visible)
        {
            return _instance;
        }

        public IModalActionButton Hidden<TProperty>(Expression<Func<TModel, TProperty>> expression, object value)
        {
            this.condition = expression.ToCondition(value);
            return _instance;
        }

        public IModalActionButton HiddenFor(Expression<Func<TModel, bool>> expression)
        {
            this.condition = expression.ToCondition();
            return _instance;
        }
    }
}
