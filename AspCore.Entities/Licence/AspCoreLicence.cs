using System;
using System.Globalization;
using System.Xml.Linq;

namespace AspCore.Entities.Licence
{
    public sealed class AspCoreLicence : IDisposable
    {
        private readonly XElement xmlData;

        private const string AspCoreLicence_Str = "AspCoreLicence";
        private const string Signature_Str = "Signature";
        private const string Expiration_Str = "Expiration";
        private const string ProjectUrl_Str = "ProjectUrl";
        private const string Versiyon_Str = "Versiyon";
        private const string Versiyon = "1.0";

        public AspCoreLicence(LicenceInfo licenceInfo)
        {
            xmlData = new XElement(AspCoreLicence_Str);

            if (licenceInfo == null)
            {
                throw new Exception("Licence Info must not null");
            }

            if (!IsSigned) SetTag(Versiyon_Str, Versiyon);
            if (!IsSigned) SetTag(ProjectUrl_Str, licenceInfo.url.Trim());
            if (!IsSigned) SetTag(Expiration_Str, licenceInfo.expiration.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture));

        }

        public AspCoreLicence(XElement xmlData)
        {
            this.xmlData = xmlData;
        }

        public string Signature
        {
            get { return GetTag(Signature_Str); }
        }

        public DateTime Expiration
        {
            get
            {
                return
                    DateTime.ParseExact(
                        GetTag(Expiration_Str) ??
                        DateTime.MaxValue.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture)
                        , "r", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
        }

        public string ProjectUrl
        {
            get
            {
                return GetTag(ProjectUrl_Str);
            }
        }

        private bool IsSigned
        {
            get { return (!string.IsNullOrEmpty(Signature)); }
        }

        public override string ToString()
        {
            return xmlData.ToString();
        }

        private void SetTag(string name, string value)
        {
            var element = xmlData.Element(name);

            if (element == null)
            {
                element = new XElement(name);
                xmlData.Add(element);
            }

            if (value != null)
                element.Value = value;
        }

        private string GetTag(string name)
        {
            var element = xmlData.Element(name);
            return element != null ? element.Value : null;
        }

        public void Dispose()
        {
        }
    }
}
