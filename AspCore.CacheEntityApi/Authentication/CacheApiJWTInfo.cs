using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;

namespace AspCore.CacheEntityApi.Authentication
{
    public class CacheApiJWTInfo : IJWTEntity
    {
        public string correlationId { get; set; }
        public Guid activeUserId { get; set; }
        public List<AuthorizedCacheNodes> AuthorizedCacheNodes { get; set; }
        public bool? isAuthorizedAllActions { get; set; }

    }
}
