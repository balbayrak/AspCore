using System;
using System.Collections.Generic;
using System.Text;
using AspCore.WebComponents.HtmlHelpers.General;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Modal.Abstract
{
    public interface IActionForm
    {
        string Id { get; set; }
        string SubmitFormId { get; set; }

        string SmallTitle { get; set; }

        string Title { get; set; }

        bool isModal { get; set; }

        EnumModalSize modalSize { get; set; }
    }
}
