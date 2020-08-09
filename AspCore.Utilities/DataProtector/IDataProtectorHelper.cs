using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Utilities.DataProtector
{
    public interface IDataProtectorHelper 
    {
        string secretKey { get; }
        IDataProtectionProvider dataProtectionProvider { get; }
        string Protect(string input);

        string UnProtect(string input);
    }
}
