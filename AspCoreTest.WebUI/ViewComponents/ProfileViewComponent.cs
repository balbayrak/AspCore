using AspCore.Caching.Abstract;
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
            return View("Default");
        }
    }
}
