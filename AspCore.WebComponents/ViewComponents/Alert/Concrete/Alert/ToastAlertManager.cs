using AspCore.WebComponents.ViewComponents.Alert.Abstract;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;

namespace AspCore.ViewComponents.Components.Alert.Concrete
{
    public class ToastAlertManager : BaseAlertManager, IAlertService
    {
        public ToastAlertManager(IAlertStorage alertStorage) : base(alertStorage)
        {

        }
        public override AlertType baseAlertType => AlertType.Toast;
    }
}

