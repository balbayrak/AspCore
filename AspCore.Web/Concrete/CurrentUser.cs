using AspCore.Caching.Abstract;
using AspCore.Entities.Constants;
using AspCore.Entities.User;
using AspCore.Web.Abstract;

namespace AspCore.Web.Concrete
{
    public class CurrentUser : ICurrentUser
    {
        private ICacheService _cache;
        public CurrentUser(ICacheService cache)
        {
            _cache = cache;
        }
        public ActiveUser currentUser
        {
            get
            {
                string tokenKey = _cache.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);
                string activeUserUId = FrontEndConstants.STORAGE_CONSTANT.COOKIE_USER + "_" + tokenKey;
                return _cache.GetObject<ActiveUser>(activeUserUId);
            }
        }
    }
}
