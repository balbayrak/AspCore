using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Entities.EntityType;

namespace AspCore.Dtos.Dto
{
    public abstract class EntityDto:IEntityDto
    {
        protected EntityDto()
        { 
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string EncryptedId { get; set; }
    }
}
