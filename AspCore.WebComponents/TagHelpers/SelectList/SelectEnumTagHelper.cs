using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.SelectList
{
    [HtmlTargetElement("select",Attributes = "selected-enum-value")]
    [HtmlTargetElement("select",Attributes = "enum-type")]
    public class SelectEnumTagHelper:TagHelper
    {
        [HtmlAttributeName("selected-enum-value")]

        public int SelectedValue { get; set; }
        [HtmlAttributeName("enum-type")]
        public Type EnumType { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            foreach (int e in Enum.GetValues(EnumType))
            {
                var op = new TagBuilder("option");
                op.Attributes.Add("value", $"{e}");
                var displayText = GetEnumFieldDisplayName(e);
                op.InnerHtml.Append(displayText);
                if (e == SelectedValue)
                    op.Attributes.Add("selected", "selected");
                output.Content.AppendHtml(op);
            }
        }
        private string GetEnumFieldDisplayName(int value)
        {
            var fieldName = Enum.GetName(EnumType, value);
            var displayName = EnumType.GetField(fieldName).GetCustomAttributes(false).OfType<DisplayAttribute>().SingleOrDefault()?.Name;
            return displayName ?? fieldName;
        }
    }


}
