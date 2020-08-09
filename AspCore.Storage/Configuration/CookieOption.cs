using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Storage.Configuration
{
    public class CookieOption : StorageOption
    {
        public bool secureCookie { get; set; } = false;
    }
}
