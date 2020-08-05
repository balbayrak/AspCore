using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Dtos.Dto;

namespace AspCore.BackendForFrontend.Entities
{
    public abstract class BaseEntityViewModel<TEntityDto>
    where TEntityDto:IEntityDto,new()
 
    {
        public TEntityDto EntityDto { get; set; }
     
    }
    
}
