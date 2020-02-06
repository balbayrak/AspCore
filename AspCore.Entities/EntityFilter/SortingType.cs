
using AspCore.Entities.EntityType;
using AspCore.Entities.Expression;

namespace AspCore.Entities.EntityFilter
{
    public class SortingType<TEntity> where TEntity : class, IEntity, new()
    {
        public string PropertyName { get; set; }

        public EnumSortingDirection SortDirection { get; set; }

        public SortingType()
        {

        }
        public SortingType(string property, EnumSortingDirection sortingDirection)
        {
            PropertyName = property;
            SortDirection = sortingDirection;
        }

    }
}