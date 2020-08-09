using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.Authentication
{
    public class AuthenticationTicketInfo
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime expires { get; set; }
    }
}
