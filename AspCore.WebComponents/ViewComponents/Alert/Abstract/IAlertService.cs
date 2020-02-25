using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using AspCore.WebComponents.ViewComponents.Alert.Concrete.Alert;

namespace AspCore.WebComponents.ViewComponents.Alert.Abstract
{
    public interface IAlertService
    {
        void Success(string Title, string Message, AlertType? alertType = null);
        void Warning(string Title, string Message, AlertType? alertType = null);
        void Error(string Title, string Message, AlertType? alertType = null);
        void Info(string Title, string Message, AlertType? alertType = null);
    }
}
