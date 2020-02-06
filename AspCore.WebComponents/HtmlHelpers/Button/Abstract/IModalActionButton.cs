using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface IModalActionButton : IActionButton<IModalActionButton>
    {
        IModalActionButton Modal(string modalid, EnumModalSize modalSize);
        IModalActionButton Modal(EnumModalSize modalSize);
        IModalActionButton BackDropStatic();
    }
}
