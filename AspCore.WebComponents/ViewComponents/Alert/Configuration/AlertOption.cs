using AspCore.WebComponents.ViewComponents.Alert.Concrete.Alert;

namespace AspCore.WebComponents.ViewComponents.Alert.Configuration
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
