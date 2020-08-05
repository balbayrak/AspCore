using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Dtos.Dto
{
    public abstract class DocumentEntityDto:EntityDto,IDocumentEntityDto
    {
        public string DocumentUrl { get; set; }
    }
}
