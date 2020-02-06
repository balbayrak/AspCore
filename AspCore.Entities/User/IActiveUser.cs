using System;
using AspCore.Entities.EntityType;

namespace AspCore.Entities.User
{
    public interface IActiveUser : IJWTEntity
    {
        public Guid id { get; set; }
    }
}
