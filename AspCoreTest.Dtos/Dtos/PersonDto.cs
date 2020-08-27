using AspCore.Dtos.Dto;
using AspCore.Mapper.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.Dtos.Dtos
{
    public class PersonDto : EntityDto, IMapFrom<Person>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual AdminDto Admin { get; set; }
    }
}
