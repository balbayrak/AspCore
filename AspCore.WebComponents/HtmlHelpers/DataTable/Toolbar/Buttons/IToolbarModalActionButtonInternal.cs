using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Toolbar.Buttons
{
    public interface IToolbarModalActionButtonInternal : IActionButtonInternal
    {
        EnumFormSide formSide { get; set; }
    }
}
