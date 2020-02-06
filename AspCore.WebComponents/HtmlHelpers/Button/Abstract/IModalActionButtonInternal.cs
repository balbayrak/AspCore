using AspCore.WebComponents.HtmlHelpers.Modal.Concrete;

namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface IModalActionButtonInternal : IActionButtonInternal
    {
        ModalUI modalui { get; set; }
        string ModalDialog();
        bool backDropStatic { get; set; }
    }
}
