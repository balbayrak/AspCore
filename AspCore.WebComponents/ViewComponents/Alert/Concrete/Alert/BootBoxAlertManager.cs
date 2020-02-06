using AspCore.WebComponents.ViewComponents.Alert.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete
{
    public class BootBoxAlertManager : BaseAlertManager, IAlertService
    {
        public BootBoxAlertManager(IAlertStorage alertStorage) : base(alertStorage)
        {

        }
        public override AlertType baseAlertType => AlertType.BootBox;
    }
}
