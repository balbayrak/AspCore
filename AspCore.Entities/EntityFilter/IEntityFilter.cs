using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AspCore.Entities.EntityType;

namespace AspCore.Entities.EntityFilter
{
    public interface IEntityFilter<TEntity> where TEntity : class, IEntity, new()
    {
        Guid id { get; set; }
        List<IncludeType<TEntity>> includes { get; set; }
        List<SortingType<TEntity>> sorters { get; set; }
        SearchType search { get; set; }
        int? page { get; set; }
        int? pageSize { get; set; }
        Expression<Func<TEntity, bool>> GetExpression();
    }
}
