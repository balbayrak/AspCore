using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.DataTable.Abstract;
using AspCore.WebComponents.HtmlHelpers.DataTable.Columns.Buttons;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class ActionBuilder<TModel>
          where TModel : class
    {
        private ITableActionColumnInternal actionColumn { get; set; }

        public ActionBuilder(ITableActionColumnInternal actioncolumn)
        {
            this.actionColumn = actioncolumn;
        }

        public ITableActionButton<IModalActionButton, TModel> ModalButton()
        {
            TableModalActionButton<TModel> act = new TableModalActionButton<TModel>();
            actionColumn.AddAction(act);
            return act;
        }

        public ITableActionButton<IDefaultActionButton, TModel> ActionButton()
        {
            TableDefaultActionButton<TModel> act = new TableDefaultActionButton<TModel>();
            actionColumn.AddAction(act);
            return act;
        }

        public ITableActionButton<IConfirmActionButton, TModel> ConfirmButton()
        {
            TableConfirmActionButton<TModel> act = new TableConfirmActionButton<TModel>();
            actionColumn.AddAction(act);
            return act;
        }

        public ITableActionButton<IDefaultActionButton, TModel> DownloadButton()
        {
            TableDownloadActionButton<TModel> act = new TableDownloadActionButton<TModel>();
            actionColumn.AddAction(act);
            return act;
        }
    }
}
