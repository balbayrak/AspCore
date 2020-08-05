using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Dtos.Dto
{
    public interface IEntityDto
    {
        Guid Id { get; set; }
        public string EncryptedId { get; set; }
    }
}
