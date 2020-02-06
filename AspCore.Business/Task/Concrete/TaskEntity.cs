using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Business.Task.Concrete
{
    public class TaskEntity<TEntity> : CoreTaskEntity
        where TEntity : class, new()
    {
        public TEntity entity { get; set; }

    }
}
