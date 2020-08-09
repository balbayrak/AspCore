using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.DocumentManagement.Validator;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;

namespace AspCore.DocumentManagement.Uploader
{
    public abstract class BaseDocumentUploader<TDocument, TOption>
         where TDocument : class, IDocument, new()
         where TOption : class, IUploaderOption, new()
    {
        protected IDocumentValidator<TDocument, TOption> _documentValidator;
        protected IApiClient _apiClient { get; private set; }
        protected string _apiControllerRoute { get; private set; }

        public BaseDocumentUploader(IDocumentValidator<TDocument, TOption> documentValidator, string uploaderKey, string apiControllerRoute)
        {
            _apiClient = ApiClientFactory.Instance.GetApiClient(uploaderKey);

            if (!apiControllerRoute.StartsWith("/"))
            {
                apiControllerRoute = "/" + apiControllerRoute;
            }
            _apiControllerRoute = apiControllerRoute;

            _documentValidator = documentValidator;
        }

        public abstract ServiceResult<TDocument> CreateDocument(IDocumentRequest<TDocument> documentRequest);

        public abstract ServiceResult<TDocument> ReadDocument(IDocumentRequest<TDocument> documentRequest);

        public abstract ServiceResult<bool> UpdateDocument(IDocumentRequest<TDocument> documentRequest);

        public abstract ServiceResult<bool> DeleteDocument(IDocumentRequest<TDocument> documentRequest);

        public ServiceResult<TDocument> Create(IDocumentRequest<TDocument> documentRequest)
        {
            ServiceResult<bool> validateResult = _documentValidator.OnValidate(documentRequest, EnumCrudOperation.CreateOperation);
            if (validateResult.IsSucceeded && validateResult.Result)
            {
                return CreateDocument(documentRequest);
            }
            else
            {
                return new ServiceResult<TDocument>
                {
                    ErrorMessage = validateResult.ErrorMessage,
                    ExceptionMessage = validateResult.ExceptionMessage
                };
            }
        }

        public ServiceResult<bool> Delete(IDocumentRequest<TDocument> documentRequest)
        {
            ServiceResult<bool> validateResult = _documentValidator.OnValidate(documentRequest, EnumCrudOperation.DeleteOperation);
            if (validateResult.IsSucceeded && validateResult.Result)
            {
                return DeleteDocument(documentRequest);
            }
            else
            {
                return validateResult;
            }
        }

        public ServiceResult<TDocument> Read(IDocumentRequest<TDocument> documentRequest)
        {
            ServiceResult<bool> validateResult = _documentValidator.OnValidate(documentRequest, EnumCrudOperation.ReadOperation);
            if (validateResult.IsSucceeded && validateResult.Result)
            {
                return ReadDocument(documentRequest);
            }
            else
            {
                return new ServiceResult<TDocument>
                {
                    ErrorMessage = validateResult.ErrorMessage,
                    ExceptionMessage = validateResult.ExceptionMessage
                };
            }
        }

        public ServiceResult<bool> Update(IDocumentRequest<TDocument> documentRequest)
        {
            ServiceResult<bool> validateResult = _documentValidator.OnValidate(documentRequest, EnumCrudOperation.UpdateOperation);
            if (validateResult.IsSucceeded && validateResult.Result)
            {
                return UpdateDocument(documentRequest);
            }
            else
            {
                return validateResult;
            }
        }
    }
}
