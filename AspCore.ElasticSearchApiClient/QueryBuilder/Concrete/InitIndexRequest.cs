using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.ElasticSearchApiClient.QueryBuilder.Concrete
{
    public class InitIndexRequest
    {
        public string indexKey { get; set; }

        public bool initializeWithData { get; set; }
    }
}
