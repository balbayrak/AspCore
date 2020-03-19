using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TseCacheManagement.CacheAccess.General
{
    public static class StringExt
    {
        private static CultureInfo culture = new CultureInfo("en-US");
        public static string ConvertTurkishCharacter(this string value)
        {
            value = value.ToLower(culture);
            value = value.Replace("ı", "i").TrimStart('.').TrimEnd('.');
            value = value.Replace("ö", "o").TrimStart('.').TrimEnd('.');
            value = value.Replace("ü", "u").TrimStart('.').TrimEnd('.');
            value = value.Replace("ş", "s").TrimStart('.').TrimEnd('.');
            value = value.Replace("ğ", "g").TrimStart('.').TrimEnd('.');
            value = value.Replace("ç", "c").TrimStart('.').TrimEnd('.');
            value = value.Replace("Ü", "U").TrimStart('.').TrimEnd('.');
            value = value.Replace("İ", "I").TrimStart('.').TrimEnd('.');
            value = value.Replace("Ö", "O").TrimStart('.').TrimEnd('.');
            value = value.Replace("Ü", "U").TrimStart('.').TrimEnd('.');
            value = value.Replace("Ş", "S").TrimStart('.').TrimEnd('.');
            value = value.Replace("Ğ", "G").TrimStart('.').TrimEnd('.');
            value = value.Replace("Ç", "C").TrimStart('.').TrimEnd('.');
            return value;
        }

    }
}
