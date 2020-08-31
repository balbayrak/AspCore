using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class ActionInfo
    {
        public ActionInfo()
        {
            PreventDefault = false;
        }
        public string actionUrl { get; set; }

        public bool PreventDefault { get; set; }

        public EnumHttpMethod methodType { get; set; }
    }
}
