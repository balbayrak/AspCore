using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Dependency.Concrete;

namespace AspCore.Utilities.DataProtector
{
    public class DataProtectorHelper : IDataProtectorHelper
    {
        private string _secretKey { get; set; }
        private IDataProtectionProvider _dataProtectionProvider { get; set; }
        public DataProtectorHelper(string secretKey)
        {
            _secretKey = secretKey;
            _dataProtectionProvider = DependencyResolver.Current.GetService<IDataProtectionProvider>();
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
