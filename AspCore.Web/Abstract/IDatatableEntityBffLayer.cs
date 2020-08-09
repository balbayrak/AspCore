using AspCore.BackendForFrontend.Abstract;
using AspCore.Dtos.Dto;
using AspCore.Entities.DataTable;
using AspCore.Entities.EntityType;
using AspCore.Mapper.Abstract;

namespace AspCore.Web.Abstract
{
    public interface IDatatableEntityBffLayer<TEntityDto,TCreatedDto,TUpdatedDto> : IEntityBffLayer<TEntityDto,TCreatedDto,TUpdatedDto>
         where TEntityDto : class, IEntityDto, new() 
         where TCreatedDto : class, IEntityDto, new()
         where TUpdatedDto : class, IEntityDto, new()
    {
        JQueryDataTablesResponse GetAll(JQueryDataTablesModel jQueryDataTablesModel);
    }

    public interface IDatatableEntityBffLayer<TEntityDto> : IDatatableEntityBffLayer<TEntityDto, TEntityDto, TEntityDto>
        where TEntityDto : class, IEntityDto, new()

    {

    }
}
