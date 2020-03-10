using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.SelectList
{
    [HtmlTargetElement("select",Attributes = "items")]
    [HtmlTargetElement("select",Attributes = "selected-list-value")]
    [HtmlTargetElement("select",Attributes = "prop-text")]
    [HtmlTargetElement("select",Attributes = "prop-value")]
    public class SelectListTagHelper : TagHelper
    {
     
        [HtmlAttributeName("items")]
        public IEnumerable<dynamic> Items { get; set; }
        [HtmlAttributeName("selected-list-value")]
        public object SelectedValue { get; set; }
        [HtmlAttributeName("prop-text")]
        public string PropText { get; set; }
        [HtmlAttributeName("prop-value")]
        public string PropValue { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    string text = string.Empty;
                    string value = string.Empty;
                    if (string.IsNullOrEmpty(PropText) && string.IsNullOrEmpty(PropValue))
                    {
                        throw new Exception("Please fill the propert value or text field");
                    }
                    if (!string.IsNullOrEmpty(PropText))
                    { 
                        text = GetPropertyValue(item, PropText);
                    }
                    if (!string.IsNullOrEmpty(PropValue))
                    { 
                        value = GetPropertyValue(item, PropValue);
                    }
                    if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(value))
                    {
                        throw new Exception("Invalid property names");
                    }
                    if (string.IsNullOrEmpty(PropText))
                    {
                        throw new Exception("Invalid fill the prop-text field.");
                    }
                    string selectedAttr = string.Empty;
                    if (SelectedValue!=null)
                    {
                        bool selected = value.Equals(SelectedValue);
                        selectedAttr = selected ? "selected='selected'" : "";
                    }
                    output.Content.AppendHtml(
                        $"<option value='{value}' {selectedAttr}>{text}</option>");
                }
            }
        }
        private object GetPropertyValue(object target, string name)
        {
           return target.GetType().GetProperty(name)?.GetValue(target, null).ToString();
        }
    }
}

