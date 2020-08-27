using AspCore.Dtos.Dto;
using AspCore.Mapper.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.Dtos.Dtos
{
    public class AdminDto : EntityDto, IMapFrom<Admin>
    {
        public string Description { get; set; }
    }
}