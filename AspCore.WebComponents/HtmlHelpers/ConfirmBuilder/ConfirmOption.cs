using AspCore.Dependency.Concrete;
using AspCore.WebComponents.HtmlHelpers.Button.Concrete;

namespace AspCore.WebComponents.HtmlHelpers.ConfirmBuilder
{
    public class ConfirmOption
    {
        public string confirmMessage { get; set; }
        public string confirmTitle { get; set; }
        public string confirmOKButtonText { get; set; }
        public string confirmCancelButtonText { get; set; }
        public ActionInfo confirmAction { get; set; }
        public string confirmCallBackFuncName { get; set; }


        public ConfirmOption()
        {
            
        }

        public ConfirmOption(string message, string title, string confirmCallBackFuncName = null) : this()
        {
            this.confirmMessage = message;
            this.confirmTitle = title;
            this.confirmCallBackFuncName = confirmCallBackFuncName;
        }

        public string ConfirmString
        {
            get
            {
                return ConfirmManagerFactory.Instance.GetConfirmString(this);
            }
        }
    }
}