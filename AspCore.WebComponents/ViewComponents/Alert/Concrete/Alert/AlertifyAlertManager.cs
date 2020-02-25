
using AspCore.WebComponents.ViewComponents.Alert.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete.Alert
{
    public class AlertifyAlertManager : BaseAlertManager, IAlertService
    {
        public AlertifyAlertManager(IAlertStorage alertStorage) : base(alertStorage)
        {

        }
        public override AlertType baseAlertType => AlertType.Alertify;

    }
}
