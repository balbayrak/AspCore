using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using AspCore.WebComponents.HtmlHelpers.DataTable.Abstract;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Toolbar.Buttons
{
    public class ToolBarDefaultActionButton : DefaultActionButton, ITableActionButton<IDefaultActionButton>, IToolbarModalActionButtonInternal
    {
        public EnumFormSide formSide { get; set; }

        public ToolBarDefaultActionButton(string id) : base(id)
        {

        }

        public IDefaultActionButton FormSide(EnumFormSide formSide)
        {
            this.formSide = formSide;
            return _instance;
        }
    }
}
