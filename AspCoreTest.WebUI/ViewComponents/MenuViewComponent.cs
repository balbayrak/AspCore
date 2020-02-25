using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.WebUI.ViewComponents
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent:ViewComponent
    {
        public MenuViewComponent()
        {
          
        }
        public IViewComponentResult Invoke()
        {
          
            return View("Default");
        }
    }
}
