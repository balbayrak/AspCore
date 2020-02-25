using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using System.Collections.Generic;
using AspCore.WebComponents.ViewComponents.Alert.Concrete.Alert;

namespace AspCore.WebComponents.ViewComponents.Alert.Abstract
{
    public abstract class BaseAlertManager 
    {
        protected IAlertStorage _alertStorage { get; set; }
        public abstract AlertType baseAlertType { get; }

        public BaseAlertManager(IAlertStorage alertStorage)
        {
            _alertStorage = alertStorage;
        }

        public void Error(string Title, string Message, AlertType? alertType = null)
        {
            AddAlert(Title, Message, AlertStyle.DANGER, alertType);
        }

        public void Info(string Title, string Message, AlertType? alertType = null)
        {
            AddAlert(Title, Message, AlertStyle.INFO, alertType);
        }

        public void Success(string Title, string Message, AlertType? alertType = null)
        {
            AddAlert(Title, Message, AlertStyle.SUCCESS, alertType);
        }

        public void Warning(string Title, string Message, AlertType? alertType = null)
        {
            AddAlert(Title, Message, AlertStyle.WARNING, alertType);
        }

        protected void AddAlert(string Title, string Message, string Type, AlertType? alertType = null)
        {
            if(!alertType.HasValue)
            {
                alertType = baseAlertType;
            }

            List<AlertInfo> alerts = null;
            alerts = _alertStorage.GetObject<List<AlertInfo>>(AlertConstants.TEMP_DATA_KEYS);
            alerts = alerts ?? new List<AlertInfo>();
            alerts.Add(new AlertInfo
            {
                Message = Message,
                Title = Title,
                Type = Type,
                AlertType = alertType.Value
            });

            _alertStorage.SetObject(AlertConstants.TEMP_DATA_KEYS, alerts);
        }

    }
}
