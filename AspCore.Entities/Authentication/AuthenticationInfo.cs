using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.Authentication
{
    public class AuthenticationInfo
    {
        public string ClientId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<string> Scope { get; set; }

        public string authenticationProvider { get; set; }
    }
}
