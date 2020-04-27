using Microsoft.AspNetCore.DataProtection;

namespace AspCore.Utilities.DataProtector
{
    public class DataProtectorHelper : IDataProtectorHelper
    {
        private string _secretKey { get; set; }
        private IDataProtectionProvider _dataProtectionProvider { get; set; }
        public DataProtectorHelper(IDataProtectionProvider dataProtectionProvider, string secretKey)
        {
            _secretKey = secretKey;
            _dataProtectionProvider = dataProtectionProvider;
        }
        public string Protect(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(_secretKey);
            return protector.Protect(input);
        }

        public string UnProtect(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(_secretKey);
            return protector.Unprotect(input);
        }
    }
}
