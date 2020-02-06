using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public interface IEntity
    {
        CoreEntityState? entityState { get; set; }
        Guid Id { get; set; }
        string EncryptedId { get; set; }

    }
}
