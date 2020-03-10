using AspCore.WebComponents.HtmlHelpers.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableLoadBuilder<TModel> : IFluentInterface where TModel : class
    {
        ITableBuilder<TModel> LoadAction(string loadAction);
    }
}
