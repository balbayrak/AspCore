using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.EntityType
{
    public class EntityHistoryChanged<TEntity>
        where TEntity : class, IEntity, new()
    {
        public EntityHistoryChanged()
        {
                
        }
        public TEntity before { get; set; }

        public TEntity after { get; set; }
    }
}
