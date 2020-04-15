using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.DataSearch.Concrete.ElasticSearch.Admin
{
    public interface IElasticSearchAdmin<TSearchableEntity> : ITransientType
             where TSearchableEntity : class, ISearchableEntity, new()
    {
        ServiceResult<bool> ResetIndex(bool initWithData);
    }
}
