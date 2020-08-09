using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Dtos.Dto
{
   public interface IDocumentEntityDto:IEntityDto
    {
        string DocumentUrl { get; set; }

    }
}
