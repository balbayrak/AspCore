using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Entities.EntityType;

namespace AspCore.Entities.DocumentType
{
    public interface IDocumentEntityRequest<TDocument, TEntity> : IDocumentRequest<TDocument>
        where TDocument : class, IDocument, new()
        where TEntity : class, IDocumentEntity, new()

    {
        TEntity entity { get; set; }

        bool deleteEntityWithDocument { get; set; }
    }
}
