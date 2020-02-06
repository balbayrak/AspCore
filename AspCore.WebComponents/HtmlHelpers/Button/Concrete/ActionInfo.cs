using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class ActionInfo
    {
        public string actionUrl { get; set; }

        public EnumHttpMethod methodType { get; set; }
    }
}
