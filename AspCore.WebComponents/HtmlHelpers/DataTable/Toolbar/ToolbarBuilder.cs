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

        public ITableActionButton<IModalActionButton> ModalActionButton(string id)
        {
            ToolBarModalActionButton act = new ToolBarModalActionButton(id);
            TableBuilder.AddToolBarAction(act);
            return act;
        }

        public ITableActionButton<IDefaultActionButton> ActionButton(string id)
        {
            ToolBarDefaultActionButton act = new ToolBarDefaultActionButton(id);
            TableBuilder.AddToolBarAction(act);
            return act;
        }
    }
}