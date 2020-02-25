using AspCore.WebComponents.ViewComponents.Alert.Abstract;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete.Alert
{

    public class SweetAlertManager : BaseAlertManager, IAlertService
    {
        public SweetAlertManager(IAlertStorage alertStorage) : base(alertStorage)
        {

        }
        public override AlertType baseAlertType => AlertType.Sweet;

    
    }
}
