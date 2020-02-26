using AspCore.WebComponents.ViewComponents.Alert.Concrete;

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
