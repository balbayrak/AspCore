using AspCore.BackendForFrontend.Concrete;
using AspCore.Dtos.Dto;
using AspCore.Entities.DataTable;
using AspCore.Entities.General;
using AspCore.WebComponents.HtmlHelpers.DataTable.Storage;
using AspCore.WebComponents.HtmlHelpers.Extensions;
using System;
using System.Collections.Generic;

namespace AspCore.Web.Concrete
{
    public abstract class BaseDatatableEntityBffLayer<TEntityDto,TCreatedDto,TUpdatedDto> : BaseEntityBffLayer<TEntityDto, TCreatedDto,TUpdatedDto>
        where TEntityDto : class,IEntityDto,new()
        where TCreatedDto : class,IEntityDto,new()
        where TUpdatedDto : class,IEntityDto,new()
    {
        public BaseDatatableEntityBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        public virtual JQueryDataTablesResponse GetAll(JQueryDataTablesModel jQueryDataTablesModel)
        {

            try
            {
                var storageObject = jQueryDataTablesModel.columnInfos.DeSerialize<TEntityDto>();
                if (storageObject != null)
                {
                    ServiceResult<List<TEntityDto>> result = GetAllAsync(jQueryDataTablesModel.ToEntityFilter(storageObject.GetSearchableColumnString())).Result;
                    if (result.IsSucceeded && result.Result != null)
                    {
                        using (var parser = new DatatableParser<TEntityDto>(result.Result, storageObject))
                        {
                            return parser.Parse(jQueryDataTablesModel, result.TotalResultCount, result.SearchResultCount);
                        }

                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
    public abstract class BaseDatatableEntityBffLayer<TEntityDto>: BaseDatatableEntityBffLayer<TEntityDto, TEntityDto, TEntityDto>
        where TEntityDto : class, IEntityDto, new()

    {
        protected BaseDatatableEntityBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
