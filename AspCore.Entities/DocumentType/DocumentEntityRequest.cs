using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Entities.EntityType;

namespace AspCore.Entities.DocumentType
{
    public class DocumentEntityRequest<TEntity> : DocumentRequest, IDocumentEntityRequest<Document, TEntity>
      where TEntity : class, IDocumentEntity, new()
    {
        public TEntity entity { get; set; }
        public bool deleteEntityWithDocument { get; set; }
    }
}
