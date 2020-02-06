using AspCore.Entities.EntityType;

namespace AspCore.Entities.EntityFilter
{
    public class IncludeType<TEntity> where TEntity : class, IEntity, new()
    {
        public string IncludeName { get; set; }

        public IncludeType(string name)
        {
            IncludeName = name;
        }
        public IncludeType()
        {

        }
    }
}