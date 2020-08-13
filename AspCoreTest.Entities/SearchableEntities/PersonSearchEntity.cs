using AspCore.Entities.EntityType;
using AspCore.Mapper.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.Entities.SearchableEntities
{
    public class PersonSearchEntity : BaseSearchableEntity, IMapFrom<Person>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
