using Microsoft.AspNetCore.DataProtection;

namespace AspCore.Utilities.DataProtector
{
    public class DataProtectorHelper : IDataProtectorHelper
    {
        public string secretKey { get; private set; }
        public  IDataProtectionProvider dataProtectionProvider { get; private set; }

        private readonly IDataProtector protector;
        public DataProtectorHelper(IDataProtectionProvider dataProtectionProvider, string secretKey)
        {
            this.secretKey = secretKey;
            this.dataProtectionProvider = dataProtectionProvider;
            protector = dataProtectionProvider.CreateProtector(secretKey);
        }
        public string Protect(string input)
        {
            return protector.Protect(input);
        }

        public string UnProtect(string input)
        {
            return protector.Unprotect(input);
        }
    }
}
