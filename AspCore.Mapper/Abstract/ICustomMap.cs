using AutoMapper;

namespace AspCore.Mapper.Abstract
{
    public interface ICustomMap
    {
        void CreateMappings(IProfileExpression configuration);
    }
}
