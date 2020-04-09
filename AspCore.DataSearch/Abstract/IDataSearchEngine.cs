using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.DataSearch.Abstract
{
    public interface IDataSearchEngine<TSearchableEntity>
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        ServiceResult<bool> Create(params TSearchableEntity[] searchableEntities);
        ServiceResult<bool> Update(params TSearchableEntity[] searchableEntities);
        ServiceResult<bool> Delete(params TSearchableEntity[] searchableEntities);
        ServiceResult<bool> ResetIndex(bool initWithData);
    }
}
