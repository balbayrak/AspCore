using AspCore.Entities.Constants;
using AspCore.Entities.User;
using AspCore.Storage.Abstract;
using Microsoft.AspNetCore.Mvc;


namespace AspCoreTest.WebUI.ViewComponents
{
    [ViewComponent(Name = "Profile")]

    public class ProfileViewComponent : ViewComponent
    {
        IStorage _storage;

        public ProfileViewComponent(IStorage storage)
        {
            _storage = storage;
        }

        public IViewComponentResult Invoke()
        {
            var user = _storage.GetObject<ActiveUser>(FrontEndConstants.STORAGE_CONSTANT.COOKIE_USER);
            return View("Default");
        }
    }
}
