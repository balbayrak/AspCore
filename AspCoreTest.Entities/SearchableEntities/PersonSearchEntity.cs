using AspCore.Entities.EntityType;

namespace AspCoreTest.Entities.SearchableEntities
{
    public class PersonSearchEntity : BaseSearchableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
