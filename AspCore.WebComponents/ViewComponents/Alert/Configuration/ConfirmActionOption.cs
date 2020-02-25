using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;

namespace AspCore.WebComponents.ViewComponents.Alert.Configuration
{
    public class ConfirmActionOption
    {
        private ConfirmType _confirmType { get; set; }
        public ConfirmType confirmType
        {
            get
            {
                return _confirmType;
            }
            set
            {
                _confirmType = value;
            }
        }
    }
}
