using AspCore.DataSearchApi.ElasticSearch.Authentication;
using AspCore.Entities.Authentication;
using AspCore.WebApi;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.Providers.Abstract;
using System;

namespace AspCoreTest.DataSearchApi.Controllers
{
    public class AccountController : BaseJWTAuthenticationController<IApiAuthenticationProvider<AuthenticationInfo, ElasticSearchApiJWTInfo>, ITokenGenerator<ElasticSearchApiJWTInfo>, AuthenticationInfo, ElasticSearchApiJWTInfo>
    {
        public override string authenticationProviderName => typeof(ElasticSearchAppSettingAuthProvider).Name;
        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}