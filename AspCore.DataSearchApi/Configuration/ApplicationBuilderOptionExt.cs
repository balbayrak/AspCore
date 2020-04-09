using AspCore.WebApi.Configuration.Options;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.DataSearchApi.Configuration
{
    public static class ApplicationBuilderOptionExt
    {
        public static ApplicationBuilderOption UseDataSearch(this ApplicationBuilderOption option, IApplicationBuilder app, Action<ElasticSearchInitializer> initializer)
        {
            ElasticSearchInitializer elasticSearchInitializer = new ElasticSearchInitializer(app);
            initializer(elasticSearchInitializer);

            return option;
        }
    }
}
