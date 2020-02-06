﻿using Microsoft.AspNetCore.Html;
using System;
using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class DownloadHtmlActionButton : DownloadActionButton
    {
        public DownloadHtmlActionButton(string id) : base(id)
        {

        }

        public DownloadHtmlActionButton(string id, string text, string iClass, string cssClass, bool blockui, string blockTarget, string actionUrl, EnumHttpMethod httpMethod = EnumHttpMethod.GET)
          : base(id, text, iClass, cssClass, blockui, blockTarget, actionUrl, httpMethod)
        {
        }

        public override IHtmlContent ToHtml()
        {
            return new HtmlString(CreateLink());
        }
    }
}

