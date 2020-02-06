using AspCore.Entities.Constants;
using AspCore.Entities.User;
using AspCore.Storage.Abstract;
using AspCore.Web.Abstract;

namespace AspCore.Web.Concrete
{
    public class CurrentUser : ICurrentUser
    {
        private IStorage _storage;
        public CurrentUser(IStorage storage)
        {
            _storage = storage;
        }
        public ActiveUser currentUser
        {
            get
            {
                string tokenKey = _storage.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);
                string activeUserUId = FrontEndConstants.STORAGE_CONSTANT.COOKIE_USER + "_" + tokenKey;
                return _storage.GetObject<ActiveUser>(activeUserUId);
            }
        }
    }
}
