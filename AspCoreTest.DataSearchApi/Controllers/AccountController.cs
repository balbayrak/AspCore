using AspCore.DataSearchApi.ElasticSearch.Authentication;
using AspCore.Entities.Authentication;
using AspCore.WebApi;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.Providers.Abstract;

namespace AspCoreTest.CacheApi.Controllers
{
    public class AccountController : BaseJWTAuthenticationController<IApiAuthenticationProvider<AuthenticationInfo, ElasticSearchApiJWTInfo>, ITokenGenerator<ElasticSearchApiJWTInfo>, AuthenticationInfo, ElasticSearchApiJWTInfo>
    {
        public override string authenticationProviderName => typeof(ElasticSearchAppSettingAuthProvider).Name;
        public AccountController()
        {

        }
    }
}