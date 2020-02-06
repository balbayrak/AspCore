using AspCore.Entities.DocumentType;
using AspCore.Entities.General;

namespace AspCore.DocumentManagement.Uploader
{
    public interface IDocumentUploader<TDocument> 
      where TDocument : IDocument
    {
        ServiceResult<TDocument> Create(IDocumentRequest<TDocument> documentRequest);
        ServiceResult<TDocument> Read(IDocumentRequest<TDocument> documentRequest);
        ServiceResult<bool> Update(IDocumentRequest<TDocument> documentRequest);
        ServiceResult<bool> Delete(IDocumentRequest<TDocument> documentRequest);
    }
}
