using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using AspCore.WebComponents.HtmlHelpers.DataTable.Abstract;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Toolbar.Buttons
{
    public class ToolBarModalActionButton : ModalActionButton, ITableActionButton<IModalActionButton>, IToolbarModalActionButtonInternal
    {
        public EnumFormSide formSide { get; set; }

        public ToolBarModalActionButton(string id) : base(id)
        {

        }

        public IModalActionButton FormSide(EnumFormSide formSide)
        {
            this.formSide = formSide;
            return _instance;
        }
    }
}
