using AspCore.Utilities;
using System.Web;

namespace AspCore.WebComponents.HtmlHelpers.ConfirmBuilder
{
    public abstract class BaseConfirmManager
    {
        public abstract ConfirmType baseConfirmType { get; }
        public string GetConfirmString(ConfirmOption confirmOption)
        {
            string callbackFunc = confirmOption.confirmCallBackFuncName;

            string actionUrl = string.Empty;
            if (confirmOption.confirmAction != null && !string.IsNullOrEmpty(confirmOption.confirmAction.actionUrl))
            {
                actionUrl = HttpUtility.HtmlEncode(confirmOption.confirmAction.actionUrl);
            }
            return "Confirm.showConfirm('" + confirmOption.confirmTitle + "','" + confirmOption.confirmMessage + "','" + actionUrl + "','" + confirmOption.confirmAction.methodType.GetDescriptionFromEnumValue().Trim() + "','" + callbackFunc + "','" + baseConfirmType.GetHashCode() + "');";
        }
    }
}
