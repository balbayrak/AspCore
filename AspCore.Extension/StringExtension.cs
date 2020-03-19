using System;
using System.Globalization;

namespace AspCore.Extension
{
    public static class StringExtension
    {
        public static bool IsValidURI(this string uri)
        {
            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                return false;
            Uri tmp;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out tmp))
                return false;
            return tmp.Scheme == Uri.UriSchemeHttp || tmp.Scheme == Uri.UriSchemeHttps;
        }

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
