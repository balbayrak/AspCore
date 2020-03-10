using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.DataTable.Abstract;
using AspCore.WebComponents.HtmlHelpers.DataTable.Toolbar.Buttons;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Toolbar
{
    public class ToolBarBuilder<TModel> where TModel : class
    {
        private TableBuilder<TModel> TableBuilder { get; set; }

        public ToolBarBuilder(TableBuilder<TModel> tableBuilder, TableExportSetting exportSetting)
        {
            TableBuilder = tableBuilder;
        }

        public IToolbarActionButton<IModalActionButton> ModalActionButton()
        {
            ToolBarModalActionButton act = new ToolBarModalActionButton(string.Empty);
            TableBuilder.AddToolBarAction(act);
            return act;
        }

        public IToolbarActionButton<IDefaultActionButton> ActionButton()
        {
            ToolBarDefaultActionButton act = new ToolBarDefaultActionButton(string.Empty);
            TableBuilder.AddToolBarAction(act);
            return act;
        }
    }
}