using AspCore.DocumentManagement.Uploader;

namespace AspCore.DocumentManagement.Validator
{
    public class BaseDocumentValidator<TOption>
        where TOption : class, IUploaderOption, new()
    {
        public TOption uploaderOption { get; private set; }
        public BaseDocumentValidator(TOption option)
        {
            uploaderOption = option;
        }
    }
}
