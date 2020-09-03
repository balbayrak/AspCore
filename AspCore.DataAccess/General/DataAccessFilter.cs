using AspCore.Entities.EntityType;
using AspCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.DataAccess.General
{
    public class DataAccessFilter<TEntity>
         where TEntity : class, IEntity
    {
        public Expression<Func<TEntity, bool>> query { get; set; }
        public Expression<Func<TEntity, bool>> searchQuery { get; set; }
        public List<SortingExpression<TEntity>> sorters { get; set; }
        public List<Expression<Func<TEntity, object>>> includes { get; set; }
        public int? pageSize { get; set; }
        public int? page { get; set; }

        public DataAccessFilter()
        {
            this.query = null;
            this.searchQuery = null;
            this.page = null;
            this.pageSize = null;
            this.sorters = null;
        }
    }
}
