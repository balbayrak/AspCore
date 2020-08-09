using System;
using AspCore.Dtos.Dto;
using AspCore.Mapper.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.Dtos.Dtos
{
    public class PersonCvDto :DocumentEntityDto,IMapFrom<PersonCv>
    {
        public string Name { get; set; }
        public Guid PersonId { get; set; }
        public virtual PersonDto Person { get; set; }

    }
}
