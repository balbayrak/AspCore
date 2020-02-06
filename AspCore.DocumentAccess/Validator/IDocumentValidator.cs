using AspCore.DocumentManagement.Uploader;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;

namespace AspCore.DocumentManagement.Validator
{
    public interface IDocumentValidator<TDocument, TOption>
        where TDocument : IDocument
        where TOption : class, IUploaderOption, new()
    {
        TOption uploaderOption { get; }
        ServiceResult<bool> OnValidate(IDocumentRequest<TDocument> documentRequest,EnumCrudOperation crudOperation);
    }
}
