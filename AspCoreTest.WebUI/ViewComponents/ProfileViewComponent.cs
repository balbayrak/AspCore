using AspCore.Caching.Abstract;
using AspCore.Entities.Constants;
using AspCore.Entities.User;
using Microsoft.AspNetCore.Mvc;


namespace AspCoreTest.WebUI.ViewComponents
{
    [ViewComponent(Name = "Profile")]

    public class ProfileViewComponent : ViewComponent
    {
        ICacheService _cache;

        public ProfileViewComponent(ICacheService cache)
        {
            _cache = cache;
        }

        public IViewComponentResult Invoke()
        {
            var user = _cache.GetObject<ActiveUser>(FrontEndConstants.STORAGE_CONSTANT.COOKIE_USER);
            return View("Default");
        }
    }
}
