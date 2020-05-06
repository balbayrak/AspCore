using AspCore.BackendForFrontend.Abstract;
using AspCore.Caching.Abstract;
using AspCore.Entities.Constants;
using AspCore.Entities.User;

namespace AspCore.BackendForFrontend.Concrete
{
    public class ApplicationCachedClient : IApplicationCachedClient
    {
        private ICacheService _cacheService;
        private ICookieService _cookieService;

        public ApplicationCachedClient(ICacheService cacheService, ICookieService cookieService)
        {
            _cacheService = cacheService;
            _cookieService = cookieService;
        }

        public string ApplicationUserKey
        {
            get
            {
                return _cookieService.GetObject<string>(ApiConstants.Api_Keys.APP_USER_STORAGE_KEY);
            }
        }

        public T GetAuthenticatedUser<T>() where T : class, IAuthenticatedUser, new()
        {
            string tokenKey = _cookieService.GetObject<string>(ApiConstants.Api_Keys.APP_USER_STORAGE_KEY);
            string activeUserUId = FrontEndConstants.STORAGE_CONSTANT.APPLICATION_USER + "_" + tokenKey;
            return _cacheService.GetObject<T>(activeUserUId);
        }
    }
}
