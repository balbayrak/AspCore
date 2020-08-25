using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class ActionInfo
    {
        public ActionInfo()
        {
            IsAction = true;
        }
        public string actionUrl { get; set; }

        public bool IsAction { get; set; }

        public EnumHttpMethod methodType { get; set; }
    }
}
