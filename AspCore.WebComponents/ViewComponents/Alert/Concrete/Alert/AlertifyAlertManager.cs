
using AspCore.WebComponents.ViewComponents.Alert.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete
{
    public class AlertifyAlertManager : BaseAlertManager, IAlertService
    {
        public AlertifyAlertManager(IAlertStorage alertStorage) : base(alertStorage)
        {

        }
        public override AlertType baseAlertType => AlertType.Alertify;

    }
}
