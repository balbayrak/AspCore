﻿using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.Buttons
{
    [HtmlTargetElement("download-button")]
    public class DownloadButtonTagHelper : ButtonTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            DownloadHtmlActionButton modalHtmlActionButton = new DownloadHtmlActionButton(id, text, iClass, cssClass, blockui, blockTarget, actionUrl, httpMethod);

            output.Content.SetHtmlContent(modalHtmlActionButton.ToHtml());
            base.Process(context, output);
        }
    }
}