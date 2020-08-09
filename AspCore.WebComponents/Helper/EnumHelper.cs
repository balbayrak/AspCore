using AspCore.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.WebComponents.Helper
{
    public static class EnumHelper
    {
        public static List<SelectListItem> GetEnumListItems(this Enum value, string defaultText, string defaultValue)
        {
            var list = new List<SelectListItem>();
            Type enumType = value.GetType();
            var enumValues = Enum.GetValues(enumType);

            foreach (var enumvalue in enumValues)
            {
                var attribute = (EnumTextAttribute)enumType.GetMember(enumvalue.ToString())[0].GetCustomAttributes(typeof(EnumTextAttribute), false).FirstOrDefault();
                var text = attribute != null ? attribute.Text : enumvalue.ToString();

                list.Add(new SelectListItem
                {
                    Text = text,
                    Value = enumvalue.GetHashCode().ToString()
                });
            }

            if (!string.IsNullOrEmpty(defaultText))
            {
                list.Add(new SelectListItem
                {
                    Text = defaultText,
                    Selected = true,
                    Value = defaultValue

                });
            }

            return list;
        }
    }
}
