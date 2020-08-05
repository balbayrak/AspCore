using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Dtos.Dto
{
   public interface ISearchableEntityDto:IEntityDto
    {
        long searchId { get; set; }

    }
}
