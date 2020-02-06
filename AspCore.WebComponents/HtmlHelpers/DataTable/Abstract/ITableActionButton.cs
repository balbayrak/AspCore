using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface ITableActionButton<T>
        where T : IActionButton<T>
    {
        T FormSide(EnumFormSide formSide);
    }
}
