using AspCore.Entities.EntityType;

namespace AspCore.Entities.EntityType
{
    public abstract class BaseViewModel<T> 
        where T : class, IEntity, new()
    {
        public T dataEntity { get; set; }
    }

}
