using AspCore.WebComponents.HtmlHelpers.DataTable.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspCore.WebComponents.HtmlHelpers.DataTable
{
    public static class MvcHtmlTableExtensions
    {
        public static ITableLoadBuilder<TModel> DataTableHelper<TModel>(this IHtmlHelper helper, string id) where TModel : class
        {
            return new TableBuilder<TModel>(id);
        }
    }
}