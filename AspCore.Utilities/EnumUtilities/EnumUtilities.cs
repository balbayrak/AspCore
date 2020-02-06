using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.Utilities
{
    public static class EnumUtil
    {
        public static List<EnumItem> GetEnumList(this Enum value)
        {
            var list = new List<EnumItem>();
            Type enumType = value.GetType();
            var enumValues = Enum.GetValues(enumType);

            foreach (var enumvalue in enumValues)
            {
                var attribute = (EnumTextAttribute)enumType.GetMember(enumvalue.ToString())[0].GetCustomAttributes(typeof(EnumTextAttribute), false).FirstOrDefault();
                var text = attribute != null ? attribute.Text : enumvalue.ToString();

                list.Add(new EnumItem
                {
                    Text = text,
                    Value = enumvalue.GetHashCode()
                });
            }

            return list;
        }

        public static List<SelectListItem> GetEnumListItems(this Enum value,string defaultText, string defaultValue)
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

            if(!string.IsNullOrEmpty(defaultText))
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

        public static string GetDescriptionFromEnumValue(this Enum value)
        {
            try
            {
                DescriptionAttribute attribute = value.GetType()
               .GetField(value.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false)
               .SingleOrDefault() as DescriptionAttribute;
                return attribute == null ? value.ToString() : attribute.Description;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static Enum GetEnumValueFromDescripton(Type enumtType,string description)
        {
            if (!enumtType.IsEnum) throw new InvalidOperationException();
            foreach (var field in enumtType.GetFields())
            {
                
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (Enum)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (Enum)field.GetValue(null);
                }
            }
            throw new ArgumentException("Description Not Found.", "description");
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
