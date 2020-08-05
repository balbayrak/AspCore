using AspCore.Mapper.Abstract;
using AutoMapper;

namespace AspCore.Mapper.Concrete
{
    internal class AutoObjectMapper:IAutoObjectMapper
    {
        public IMapper Mapper { get; }
        public AutoObjectMapper(IMapper mapper)
        {
            Mapper = mapper;
        }
    }
}
