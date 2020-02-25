using AspCore.WebComponents.ViewComponents.Alert.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete.Alert
{
    public class ToastAlertManager : BaseAlertManager, IAlertService
    {
        public ToastAlertManager(IAlertStorage alertStorage) : base(alertStorage)
        {

        }
        public override AlertType baseAlertType => AlertType.Toast;
    }
}

