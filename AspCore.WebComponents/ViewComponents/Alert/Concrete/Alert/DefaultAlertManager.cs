using AspCore.WebComponents.ViewComponents.Alert.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete
{
    public class DefaultAlertManager : BaseAlertManager, IAlertService
    {
        public DefaultAlertManager(IAlertStorage alertStorage) : base(alertStorage)
        {
                
        }
        public override AlertType baseAlertType => AlertType.Default;
    }
}
