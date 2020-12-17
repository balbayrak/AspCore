using System;
using System.Linq.Expressions;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableCheckColumn : ITableColumn<ITableCheckColumn>
    {
        ITableCheckColumn CheckAllEnabled();
       

    }
}
