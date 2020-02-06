using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.DataTable;
using AspCore.Entities.EntityType;

namespace AspCore.Web.Abstract
{
    public interface IDatatableEntityBffLayer<TViewModel, TEntity> : IEntityBffLayer<TViewModel, TEntity>
         where TViewModel : BaseViewModel<TEntity>
         where TEntity : class, IEntity, new()
    {
        JQueryDataTablesResponse GetAll(JQueryDataTablesModel jQueryDataTablesModel);
    }
}
