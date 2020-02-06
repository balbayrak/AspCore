using AspCore.WebComponents.HtmlHelpers.DataTable.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspCore.WebComponents.HtmlHelpers.DataTable
{
    public static class MvcHtmlTableExtensions
    {
        public static ITableBuilder<TModel> DataTableHelper<TModel>(this IHtmlHelper helper) where TModel : class
        {
            return new TableBuilder<TModel>();
        }
    }
}