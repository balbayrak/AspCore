using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Utilities.DataProtector
{
    public interface IDataProtectorHelper 
    {
        string Protect(string input);

        string UnProtect(string input);
    }
}
