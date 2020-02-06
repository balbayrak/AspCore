using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCore.ViewComponents.Components.Alert.Concrete;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;

namespace AspCore.ViewComponents.Components.Alert.Configuration
{
    public class AlertOption
    {
        public EnumAlertStorage alertStorage { get; set; }
        public string baseNameSpace { get; set; }
        private AlertType _alertType { get; set; }
        public AlertType alertType
        {
            get
            {
                return _alertType;
            }
            set
            {
                _alertType = value;
            }
        }


    }
}
