using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;

namespace AspCore.DataSearchApi.ElasticSearch.Authentication
{
    public class ElasticSearchApiJWTInfo : IJWTEntity
    {
        public string correlationId { get; set; }
        public Guid activeUserId { get; set; }
        public List<AuthorizedElasticSearchIndex> AuthorizedElasticSearchIndices { get; set; }
        public bool? isAuthorizedAllActions { get; set; }

    }
}
