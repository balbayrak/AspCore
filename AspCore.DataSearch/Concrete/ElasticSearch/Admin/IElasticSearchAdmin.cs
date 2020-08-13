using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.DataSearch.Concrete.ElasticSearch.Admin
{
    public interface IElasticSearchAdmin<TSearchableEntity> : ITransientType
             where TSearchableEntity : class, ISearchableEntity, new()
    {
        Task<ServiceResult<bool>> ResetIndex(bool initWithData);
    }
}
