using AspCore.DocumentManagement.Validator;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;

namespace AspCore.DocumentManagement.Uploader
{
    public class DocumentUploader<TDocument, TOption> : BaseDocumentUploader<TDocument, TOption>, IDocumentUploader<TDocument>
         where TDocument : class, IDocument, new()
         where TOption : class, IUploaderOption, new()
    {

        public DocumentUploader(IDocumentValidator<TDocument,TOption> validator, string apiKey, string apiControllerRoute) : base(validator,apiKey, apiControllerRoute)
        {

        }

        public override ServiceResult<TDocument> CreateDocument(IDocumentRequest<TDocument> documentRequest)
        {
            _apiClient.apiUrl = _apiControllerRoute + "/" + ApiConstants.DocumentApi_Urls.CREATE;
            return _apiClient.PostRequest<ServiceResult<TDocument>>(documentRequest).Result;
        }

        public override ServiceResult<bool> DeleteDocument(IDocumentRequest<TDocument> documentRequest)
        {
            _apiClient.apiUrl = _apiControllerRoute + "/" + ApiConstants.DocumentApi_Urls.DELETE;
            return _apiClient.PostRequest<ServiceResult<bool>>(documentRequest).Result;
        }

        public override ServiceResult<TDocument> ReadDocument(IDocumentRequest<TDocument> documentRequest)
        {
            _apiClient.apiUrl = _apiControllerRoute + "/" + ApiConstants.DocumentApi_Urls.READ;
            return _apiClient.PostRequest<ServiceResult<TDocument>>(documentRequest).Result;
        }

        public override ServiceResult<bool> UpdateDocument(IDocumentRequest<TDocument> documentRequest)
        {
            _apiClient.apiUrl = _apiControllerRoute + "/" + ApiConstants.DocumentApi_Urls.UPDATE;
            return _apiClient.PostRequest<ServiceResult<bool>>(documentRequest).Result;
        }
    }
}
