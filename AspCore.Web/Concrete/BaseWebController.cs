using AspCore.ApiClient.Entities.Concrete;
using AspCore.BackendForFrontend.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Storage.Abstract;
using AspCore.Utilities.DataProtector;
using AspCore.Utilities.MimeMapping;
using AspCore.WebComponents.ViewComponents.Alert.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using AspCore.Authentication.Abstract;
using AspCore.Authentication.Concrete;

namespace AspCore.Web.Concrete
{
    public abstract class BaseWebController<TDocument, TDocumentRequest> : Controller
        where TDocument : class, IDocument, new()
        where TDocumentRequest : class, IDocumentRequest<TDocument>, new()
    {
        protected readonly IStorage Storage;
        protected IAlertService AlertService;
        private readonly IUserBffLayer _userBffLayer;
        protected IDataProtectorHelper DataProtectorHelper;
        protected IDocumentBffLayer<TDocument> DocumentHelper;
        protected IConfigurationAccessor ConfigurationAccessor;

        protected BaseWebController()
        {
            _userBffLayer = DependencyResolver.Current.GetService<IUserBffLayer>();
            Storage = DependencyResolver.Current.GetService<IStorage>();
            AlertService = DependencyResolver.Current.GetService<IAlertService>();
            DataProtectorHelper = DependencyResolver.Current.GetService<IDataProtectorHelper>();
            DocumentHelper = DependencyResolver.Current.GetService<IDocumentBffLayer<TDocument>>();
            ConfigurationAccessor = DependencyResolver.Current.GetService<IConfigurationAccessor>();
        }
     
        protected ActiveUser activeUser
        {
            get
            {
                string tokenKey = Storage.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);
                string activeUserUId = FrontEndConstants.STORAGE_CONSTANT.COOKIE_USER + "_" + tokenKey;
                return Storage.GetObject<ActiveUser>(activeUserUId);
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string tokenKey = Storage.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);
            var token = Storage.GetObject<AuthenticationTokenResponse>(tokenKey);
            if (token != null)
            {
                string activeUserUId = FrontEndConstants.STORAGE_CONSTANT.COOKIE_USER + "_" + tokenKey;
                var activeUser = Storage.GetObject<ActiveUser>(activeUserUId);
                if (activeUser == null)
                {
                    ServiceResult<ActiveUser> userResult = _userBffLayer.GetClientInfo(token).Result;

                    if (userResult != null && userResult.IsSucceeded && userResult.Result != null)
                    {
                        Storage.SetObject(activeUserUId, userResult.Result, DateTime.Now.AddHours(1), false);
                    }
                }
            }
            else
            {
                Response.Redirect();
            }
           

            base.OnActionExecuting(context);
        }

        [HttpGet]
        public IActionResult DownloadDocument(string documentUrl)
        {
            if (!string.IsNullOrEmpty(documentUrl))
            {
                ServiceResult<TDocument> documentResult = DocumentHelper.GetDocument(new TDocumentRequest
                {
                    document = new TDocument
                    {
                        url = documentUrl
                    }
                });

                if (documentResult.IsSucceededAndDataIncluded())
                {
                    IMimeMappingService mimeMappingService = DependencyResolver.Current.GetService<IMimeMappingService>();
                    return File(documentResult.Result.content, mimeMappingService.Map(documentResult.Result.name), documentResult.Result.name);
                }
            }

            return BadRequest(string.Format(FrontEndConstants.ERROR_MESSAGES.PARAMETER_IS_NULL, nameof(documentUrl)));
        }
    }
}
