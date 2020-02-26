using System;
using System.Collections.Generic;
using System.Text;
using AspCore.WebComponents.ViewComponents.Alert.Abstract;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace AspCore.WebComponents.ViewComponents
{
    [ViewComponent(Name = "Alert")]
   public class AlertViewComponent:ViewComponent
    {
        private readonly IAlertStorage _alertStorage;

        public AlertViewComponent(IAlertStorage alertStorage)
        {
            _alertStorage = alertStorage;
        }
        public IViewComponentResult Invoke()
        {
            var alerts = _alertStorage.GetObject<List<AlertInfo>>(AlertConstants.TEMP_DATA_KEYS);
            return View("~/ViewComponents/Views/Shared/Components/Alert/Default.cshtml", alerts);
        }
    }
}
